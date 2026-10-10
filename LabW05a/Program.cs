/****************/
// Mã số sinh viên: 202418834
// Họ tên: Trần Tuấn Anh
/****************/

using System;
using System.Collections.Generic;
using System.Text;

namespace LabW05a
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== CHƯƠNG TRÌNH QUẢN LÝ THIẾT BỊ VÀ BẢO TRÌ ===");

            // 1. Tạo các thiết bị theo yêu cầu kiểm thử
            Computer comp1 = new Computer("C01", "Dell Optiplex", 2020, 15000000m, DeviceStatus.Active, 16, "Core i5", false);
            Computer comp2 = new Computer("C02", "Alienware", 2023, 45000000m, DeviceStatus.Active, 32, "Core i9", true); // Có GPU rời

            Printer printer1 = new Printer("P01", "Canon LBP2900", 2021, 3500000m, DeviceStatus.Active, PrinterType.Laser, 50000, false);
            NetworkPrinter printer2 = new NetworkPrinter("P02", "Epson L805", 2018, 6500000m, DeviceStatus.UnderMaintenance, PrinterType.Inkjet, 120000, true); // Máy in màu, in > 100k trang, có mạng

            Projector proj1 = new Projector("PR01", "Sony VPL-EX", 2019, 12000000m, DeviceStatus.Active, 3500, 3200); // Đã sử dụng > 3000 giờ

            // 2. Tạo phòng thực hành
            LabRoom room1 = new LabRoom("R01", "Phòng Máy 1", 30);
            LabRoom room2 = new LabRoom("R02", "Phòng Máy 2", 20);

            // 3. Thêm thiết bị vào phòng
            try {
                room1.AddDevice(comp1);
                room1.AddDevice(comp2);
                room1.AddDevice(printer1);
                
                room2.AddDevice(printer2);
                room2.AddDevice(proj1);
                Console.WriteLine("Thêm thiết bị thành công.");
            } catch (Exception ex) {
                Console.WriteLine($"Lỗi khi thêm thiết bị: {ex.Message}");
            }

            // 4. Thử thêm thiết bị trùng mã
            Console.WriteLine("\n[Kiểm tra] Thêm thiết bị trùng mã:");
            try {
                room1.AddDevice(new Computer("C01", "Laptop HP", 2022, 20000000m, DeviceStatus.Active, 8, "Core i5", false));
            } catch (Exception ex) {
                Console.WriteLine($"-> Lỗi dự kiến: {ex.Message}");
            }

            // 5. In danh sách thiết bị trong từng phòng
            Console.WriteLine("\n=== DANH SÁCH THIẾT BỊ ===");
            List<LabRoom> allRooms = new List<LabRoom> { room1, room2 };
            foreach (var room in allRooms)
            {
                Console.WriteLine($"--- {room.RoomName} (Mã: {room.RoomId}) ---");
                foreach (var device in room.GetAllDevices())
                {
                    Console.WriteLine(device.ToString());
                }
            }

            // 6. Tính tổng chi phí bảo trì dự kiến của mỗi phòng
            Console.WriteLine("\n=== CHI PHÍ BẢO TRÌ DỰ KIẾN ===");
            foreach (var room in allRooms)
            {
                Console.WriteLine($"{room.RoomName}: {room.CalculateAnnualMaintenanceCost():N0} VND");
            }

            // 7. Liệt kê thiết bị cần bảo trì
            Console.WriteLine("\n=== DANH SÁCH THIẾT BỊ CẦN BẢO TRÌ ===");
            foreach (var room in allRooms)
            {
                var maintenanceDevices = room.GetDevicesRequiringMaintenance();
                if (maintenanceDevices.Count > 0)
                {
                    Console.WriteLine($"Tại {room.RoomName}:");
                    foreach (var device in maintenanceDevices)
                    {
                        Console.WriteLine($" - {device.DeviceName} ({device.DeviceId}) | Lý do: {(device.Status == DeviceStatus.UnderMaintenance ? "Đang bảo trì" : "Sử dụng > 5 năm")}");
                    }
                }
            }

            // 8. Kết nối mạng cho các đối tượng thực thi INetworkable
            Console.WriteLine("\n=== KẾT NỐI MẠNG ===");
            try {
                comp1.Connect("192.168.1.10");
                comp2.Connect("192.168.1.11");
                printer2.Connect("192.168.1.50");
                Console.WriteLine("Đã kết nối mạng thành công cho các thiết bị hỗ trợ.");
            } catch (Exception ex) {
                Console.WriteLine($"Lỗi kết nối: {ex.Message}");
            }

            // 9. Duyệt các thiết bị mạng thông qua kiểu INetworkable
            Console.WriteLine("\n=== DANH SÁCH THIẾT BỊ TRÊN MẠNG (INetworkable) ===");
            foreach (var room in allRooms)
            {
                foreach (var device in room.GetAllDevices())
                {
                    if (device is INetworkable networkDevice && networkDevice.IsConnected)
                    {
                        Console.WriteLine($"- {(device as Device)?.DeviceName} đang kết nối tại IP: {networkDevice.IpAddress}");
                    }
                }
            }
            
            Console.ReadLine();
        }
    }
}
