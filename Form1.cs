using DPUruNet;
using DPCtlXUru;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Drawing.Imaging;

namespace Atendance_System
{
    public partial class Att_Enterance : Form
    {
        public delegate void DisplayCapture(Bitmap bitmap);
        public delegate void CloseMessage(string closeMessage);
        Enroll_Form enroll_Form;
        public Reader reader;
        private const int PROBABILITY_ONE = 0x7fffffff;
        private List<Fmd> preEnrollment;
        int count;

        [DllImport("kernel32.dll")] private static extern bool AllocConsole();

        public Att_Enterance()
        {
            AllocConsole();
            InitializeComponent();
            ReaderCollection readers = ReaderCollection.GetReaders();
            reader = readers[0];
            preEnrollment = new List<Fmd>();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            if (reader != null)
                label3.Text = $"Reader Detected: {reader.Description.SerialNumber}";
            else
            {
                label3.Text = $"Reader not detected";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = (DateTime.Now.ToString("HH:mm:ss"));
        }
        private void startCapture()
        {
            if (reader == null)
            {
                MessageBox.Show("Reader not initialized.");
                return;
            }

            reader.Open(DPUruNet.Constants.CapturePriority.DP_PRIORITY_EXCLUSIVE);

            reader.On_Captured += new Reader.CaptureCallback(OnCaptured);
            // event is subscribed to event handler( callback delegate )=>as onCaptured has same sig as of the delegate CaptureCallback
            reader.CaptureAsync(DPUruNet.Constants.Formats.Fid.ANSI, DPUruNet.Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, reader.Capabilities.Resolutions[0]);
        }
        private void StoreFmdInDatabase(byte[] fmdBytes, string Xml)
        {   string joining_Date = DateTime.Now.ToString("F");
            string _name= enroll_Form.getName();
            Console.WriteLine(_name);
            bool is_enrolled = false;
            string cs = "Data Source=DESKTOP-1907SQ5;Initial Catalog=Attendance;Integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            string query = "INSERT INTO Employees (Emp_Name,Emp_Fmd,Emp_Joining) VALUES (@name , @Xml , @Joining)";
            string query2 = "Select Emp_Fmd from Employees";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlCommand cmd2 = new SqlCommand(query2, con);
            cmd.Parameters.AddWithValue("@name", _name);
            cmd.Parameters.AddWithValue("@Xml", Xml);
            cmd.Parameters.AddWithValue("@Joining", joining_Date);
            //cmd2.Parameters.AddWithValue("@Fmd", fmdBytes);
            //cmd2.Parameters.AddWithValue("@Xml", Xml);
            con.Open();

            // Retrieve all FMDs from the database
            SqlDataReader dr = cmd2.ExecuteReader();
            List<Fmd> existingFmds = new List<Fmd>();

            while (dr.Read())
            {
                string xmlData = dr["Emp_Fmd"].ToString();
                existingFmds.Add(Fmd.DeserializeXml(xmlData));
            }
            dr.Close();
            int checkNo = 0;
            foreach (var existingFmd in existingFmds)
            {
                CompareResult compareResult = Comparison.Compare(existingFmd, 0, Fmd.DeserializeXml(Xml), 0);
                Console.WriteLine($"Checking {++checkNo} FMD in DB || Dissimilarity Score => {compareResult.Score} ");
                if (compareResult.Score <= PROBABILITY_ONE / 100000)
                {
                    is_enrolled = true;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\t\t\t\t\t*MATCHED");
                    Console.ResetColor();
                }

            }
            checkNo = 0;
            con.Close();

            con.Open();
            if (!is_enrolled)
            {

                int nq = cmd.ExecuteNonQuery();
                if (nq > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Data Entered in SqlServer Base");
                    Console.WriteLine($"Rows Affected: {nq}");
                    Console.WriteLine($"Data: {fmdBytes.LongLength}");
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.ResetColor();
                    MessageBox.Show($"Data of {_name} has been stored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CloseMessage _closeMessage = enroll_Form.closeMessage;
                    _closeMessage("Stored");
                }
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Already Enrolled in Data Base");
                Console.ResetColor();
                MessageBox.Show($"Data against this Finger is already stored", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseMessage _closeMessage = enroll_Form.closeMessage;
                _closeMessage("Stored");
            }

            con.Close();
        }

        //Helper method to create a bitmap from the raw image data
        //Provided CreateBitmap method
        public Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];

            }

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }

            bmp.UnlockBits(data);

            return bmp;
        }
        private void OnCaptured(CaptureResult captureResult)
        {
            //isMatched(captureResult);
            DisplayCapture displayCapture = enroll_Form._displayCapture; // delegate subscription
            DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, DPUruNet.Constants.Formats.Fmd.ANSI);
            Console.WriteLine($"{resultConversion.Data.ViewCount} ==> resultConversionCount");

            foreach (Fid.Fiv view in captureResult.Data.Views)
            {
                Bitmap bitmap = CreateBitmap(view.RawImage, view.Width, view.Height);
                displayCapture(bitmap);
            }
            if (resultConversion.ResultCode != DPUruNet.Constants.ResultCode.DP_SUCCESS)
            {
                MessageBox.Show(resultConversion.ResultCode.ToString());
                return;
            }



            if (captureResult.ResultCode != DPUruNet.Constants.ResultCode.DP_SUCCESS)
            {
                MessageBox.Show("-Error: " + captureResult.ResultCode);
                return;
            }

            preEnrollment.Add(resultConversion.Data);
            Console.WriteLine($"{preEnrollment.Count} ==> PreEnrollCount");
            count++;


            //if (count >= 2) // Create enrollment does not need it, as I will not create an Enrollment Till the Data Minutae is Enough
            {
                DataResult<Fmd> resultEnrollment = Enrollment.CreateEnrollmentFmd(DPUruNet.Constants.Formats.Fmd.ANSI, preEnrollment);
                Console.WriteLine($"{resultEnrollment.ResultCode}  ==> ResultEnrollment Code");
                if (resultEnrollment.ResultCode == DPUruNet.Constants.ResultCode.DP_SUCCESS)
                {
                    string Xml = Fmd.SerializeXml(resultEnrollment.Data);
                    Console.WriteLine(Xml);
                    Fmd check = Fmd.DeserializeXml(Xml);
                    Console.WriteLine($"{check.Width} = {resultEnrollment.Data.Width}");
                    StoreFmdInDatabase(resultEnrollment.Data.Bytes, Xml);

                    preEnrollment.Clear();
                    count = 0;
                }
                else if (resultEnrollment.ResultCode == DPUruNet.Constants.ResultCode.DP_ENROLLMENT_INVALID_SET)
                {
                    Console.WriteLine($"-Error in Enrollment");
                    preEnrollment.Clear();
                    count = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startCapture();
            this.Hide();
            if (enroll_Form == null || enroll_Form.IsDisposed)
            {
                enroll_Form = new Enroll_Form(this);
            }
            enroll_Form.Show();
        }
    }
}