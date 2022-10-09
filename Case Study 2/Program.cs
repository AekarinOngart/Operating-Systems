/* สมาชิกภายในกลุ่ม
64015019  นายจารุพัฒน์ เคนพรม
64015051  นายเตชินท์ ไม้ทอง
64015102  นายพิสิฐพงศ์ พิสิฐแก้วเพชร
64015112  นายภูมิพัฒน์ ลาลุน
64015163  นายอภิสิทธิ์ชัย ทองโต
ุ64015172  นายเอกรินทร์ องอาจ */
using System;
using System.Threading;

namespace OS_Problem_02
{
    class Thread_safe_buffer
    {
        static int[] TSBuffer = new int[10]; // ตัวแปรสำหรับเก็บข้อมูล 10 ช่อง
        static int Front = 0; // ตัวแปรเก็บค่า index ของช่องแรกของ buffer
        static int Back = 0; // ตัวแปรเก็บค่า index ของช่องสุดท้ายของ buffer
        static int Count = 0; // ตัวแปรเก็บค่าจำนวนของข้อมูลที่อยู่ใน buffer
        static object _Lock = new object(); // ตัวแปรสำหรับ lock การใช้งานของ thread
        static int exit_flag = 0; // ตัวแปรสำหรับเก็บค่าเพื่อใช้ในการออกจาก loop ของ thread

        static void EnQueue(int eq) // ฟังก์ชันสำหรับเพิ่มข้อมูลเข้าไปใน buffer
        {
            lock (_Lock) // ใช้ lock ในการใช้งานของ thread
            {
                while ((Count == TSBuffer.Length) && (exit_flag == 0)) // ถ้า buffer เต็ม
                {
                    Console.WriteLine("Buffer is full, waiting for dequeue"); // แสดงข้อความว่า buffer เต็ม
                    Monitor.Wait(_Lock); // ให้ thread นั้นรอ
                }
                if (exit_flag == 0) // ถ้า thread ยังไม่ถึงจุดที่จะออกจาก loop
                {
                    TSBuffer[Back] = eq; // เพิ่มข้อมูลเข้าไปใน buffer
                    Back = (Back + 1) % TSBuffer.Length; // กำหนดค่า index ของช่องสุดท้ายใหม่
                    Count++; // เพิ่มจำนวนข้อมูลใน buffer
                    Console.WriteLine("Enqueue {0}", eq); // แสดงข้อความว่าเพิ่มข้อมูลเข้าไปใน buffer
                    Monitor.PulseAll(_Lock); // แจ้งให้ thread อื่นที่รอทำงานต่อ
                }
            }
        }

        static int DeQueue() // ฟังก์ชันสำหรับดึงข้อมูลออกจาก buffer
        {
            int dq = 0; // ตัวแปรเก็บค่าข้อมูลที่ดึงออกจาก buffer
            lock (_Lock) // ใช้ lock ในการใช้งานของ thread
            {
                while ((Count == 0) && (exit_flag == 0)) // ถ้า buffer ว่าง
                {
                    Console.WriteLine("Buffer is empty, waiting for enqueue"); // แสดงข้อความว่า buffer ว่าง
                    Monitor.Wait(_Lock); // ให้ thread นั้นรอ
                }
                if (exit_flag == 0) // ถ้า thread ยังไม่ถึงจุดที่จะออกจาก loop
                {
                    dq = TSBuffer[Front]; // ดึงข้อมูลออกจาก buffer
                    Front = (Front + 1) % TSBuffer.Length; // กำหนดค่า index ของช่องแรกใหม่
                    Count--; // ลดจำนวนข้อมูลใน buffer
                    Console.WriteLine("Dequeue {0}", dq); // แสดงข้อความว่าดึงข้อมูลออกจาก buffer
                    Monitor.PulseAll(_Lock); // แจ้งให้ thread อื่นที่รอทำงานต่อ
                }
            }
            return dq; // ส่งค่าข้อมูลที่ดึงออกมากลับไป
        }

        static void th01() // ฟังก์ชันสำหรับ thread ที่ทำงานเพิ่มข้อมูลเข้าไปใน buffer
        {
            for (int i = 1; i <= 51; i++) // ใช้ loop เพื่อเพิ่มข้อมูลเข้าไปใน buffer
            {
                EnQueue(i); // เรียกใช้ฟังก์ชัน EnQueue
                Thread.Sleep(5); // ให้ thread นี้หยุดทำงาน 5 มิลลิวินาที
                if (exit_flag == 1) // ถ้า thread ถึงจุดที่จะออกจาก loop
                {
                    break; // ออกจาก loop
                }
            }
        }

        static void th011() // ฟังก์ชันสำหรับ thread ที่ทำงานเพิ่มข้อมูลเข้าไปใน buffer
        {
            for (int i = 100; i <= 151; i++) // ใช้ loop เพื่อเพิ่มข้อมูลเข้าไปใน buffer
            {
                EnQueue(i); // เรียกใช้ฟังก์ชัน EnQueue
                Thread.Sleep(5); // ให้ thread นี้หยุดทำงาน 5 มิลลิวินาที
                if (exit_flag == 1) // ถ้า thread ถึงจุดที่จะออกจาก loop
                {
                    break; // ออกจาก loop
                }
            }
        }

        static void th02(object t) // ฟังก์ชันสำหรับ thread ที่ทำงานดึงข้อมูลออกจาก buffer
        {
            int i; // ตัวแปรเก็บค่าจำนวนข้อมูลที่จะดึงออกจาก buffer
            int j; // ตัวแปรเก็บค่าจำนวนข้อมูลที่จะดึงออกจาก buffer

            for (i=0; i< 60; i++) // ใช้ loop เพื่อดึงข้อมูลออกจาก buffer
            {
                j = DeQueue(); // เรียกใช้ฟังก์ชัน DeQueue
                Thread.Sleep(5); // ให้ thread นี้หยุดทำงาน 5 มิลลิวินาที
                if (exit_flag == 1) // ถ้า thread ถึงจุดที่จะออกจาก loop
                {
                    break; // ออกจาก loop
                }
                Console.WriteLine("j={0}, thread:{1}", j, t); // แสดงข้อความว่าดึงข้อมูลออกจาก buffer
                Thread.Sleep(100); // ให้ thread นี้หยุดทำงาน 100 มิลลิวินาที
            }
        }
        static void Main(string[] args) // ฟังชั่นหลัก
        {
            Thread t1 = new Thread(th01); // สร้าง thread ที่จะทำงานเพิ่มข้อมูลเข้าไปใน buffer
            Thread t11 = new Thread(th011); // สร้าง thread ที่จะทำงานเพิ่มข้อมูลเข้าไปใน buffer
            Thread t2 = new Thread(th02); // สร้าง thread ที่จะทำงานดึงข้อมูลออกจาก buffer
            Thread t21 = new Thread(th02); // สร้าง thread ที่จะทำงานดึงข้อมูลออกจาก buffer
            Thread t22 = new Thread(th02); // สร้าง thread ที่จะทำงานดึงข้อมูลออกจาก buffer

            t1.Start(); // เริ่มทำงาน thread ที่สร้างไว้
            t11.Start(); // เริ่มทำงาน thread ที่สร้างไว้
            t2.Start(1); // เริ่มทำงาน thread ที่สร้างไว้
            t21.Start(2); // เริ่มทำงาน thread ที่สร้างไว้
            t22.Start(3); // เริ่มทำงาน thread ที่สร้างไว้

            Console.ReadKey(); // รอการกดปุ่มจากผู้ใช้
            exit_flag = 1; // กำหนดให้ thread ที่ทำงานเพิ่มข้อมูลเข้าไปใน buffer ออกจาก loop
            lock (_Lock) // ใช้ lock เพื่อป้องกันการเข้าถึงข้อมูลใน buffer โดย thread อื่น
            {
                Monitor.PulseAll(_Lock); // ให้ thread ที่รอการเข้าถึงข้อมูลใน buffer ทำงานต่อ
            }
        }
    }
}