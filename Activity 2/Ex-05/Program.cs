/* สมาชิกภายในกลุ่ม
64015019  นายจารุพัฒน์ เคนพรม
64015051  นายเตชินท์ ไม้ทอง
64015102  นายพิสิฐพงศ์ พิสิฐแก้วเพชร
64015112  นายภูมิพัฒน์ ลาลุน
64015163  นายอภิสิทธิ์ชัย ทองโต
ุ64015172  นายเอกรินทร์ องอาจ */
using System;
using System.Diagnostics;
using System.Threading;

namespace OS_Sync_05
{
	class Program
	{
		private static string x = "";
		private static int exitflag = 0; 
		private static int updateFlag = 0;
		private static Object _Lock = new object();

		static void ThReadX(object i)
		{
			while (exitflag == 0)
				lock (_Lock) // 
				{
					if (x != "exit" && updateFlag == 1)
					{
						Console.WriteLine("***Thread {0} : x = {1}***", i, x);
						updateFlag = 0;
					}
				}
			Console.WriteLine("---Thread {0} exit---", i);
		}

		static void ThWriteX()
		{
			while (exitflag == 0)
			{
				lock (_Lock)
				{
					Console.Write("Input: ");
					x = Console.ReadLine();
					updateFlag = 1;
				}
				if (x == "exit")
				{
					exitflag = 1;
				}
			}
		}
		static void Main(string[] args)
		{
			Thread A = new Thread(ThWriteX);
			Thread B = new Thread(ThReadX);
			Thread C = new Thread(ThReadX);
			Thread D = new Thread(ThReadX);

			A.Start();
			B.Start(1);
			C.Start(2);
			D.Start(3);
		}
	}
}
