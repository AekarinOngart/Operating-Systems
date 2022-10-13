/* สมาชิกภายในกลุ่ม
64015019  นายจารุพัฒน์ เคนพรม
64015051  นายเตชินท์ ไม้ทอง
64015102  นายพิสิฐพงศ์ พิสิฐแก้วเพชร
64015112  นายภูมิพัฒน์ ลาลุน
64015163  นายอภิสิทธิ์ชัย ทองโต
ุ64015172  นายเอกรินทร์ องอาจ */
namespace OS_sync_01
{
    class Program
    {
        private static string x = "";

        private static int exitflag = 0; // 0 = not exit, 1 = exit

        private static int output = 1;

        private static int input = 0;

        private static Semaphore s; // สร้างตัวแปร s ขึ้นมาเป็น Semaphore

        static void ThReadX() // สร้าง Thread สำหรับอ่านข้อมูล
        {
            while (exitflag == 0)
            {
                s.WaitOne();
                if (output == 0)
                {
                    Console.WriteLine("X = {0}", x);
                }
                output = 1;
                input = 0;
                s.Release();
            }
        }

        static void ThWriteX()
        {
            while (exitflag == 0)
            {
                if (input == 0)
                {
                    Console.Write("Input: ");
                    x = Console.ReadLine(); // รับข้อมูลจากคีย์บอร์ด
                    output = 0; // ตั้งค่าให้ Thread อ่านข้อมูลทำงาน
                    input = 1; // ตั้งค่าให้ Thread เขียนข้อมูลไม่ทำงาน
                }
                if (x == "exit")
                {
                    exitflag = 1; // exitflag เป็น 1 เพื่อออกจากการทำงาน
                    Console.WriteLine("Thread 1 exit");
                }
            }
        }

        static void Main(string[] args)
        {
            Thread A = new Thread(ThReadX); // สร้าง Thread อ่านข้อมูล
            Thread B = new Thread(ThWriteX); // สร้าง Thread เขียนข้อมูล

            s = new Semaphore(1, 1); // สร้าง Semaphore สำหรับควบคุมการเข้าถึงข้อมูล

            A.Start(); // เริ่ม Thread อ่านข้อมูล
            B.Start(); // เริ่ม Thread เขียนข้อมูล
        }
    }
}
