/****************/
// Mã số sinh viên: 202418834
// Họ tên: Trần Tuấn Anh
/****************/

using System;
using System.Collections.Generic;

namespace LabW05a
{
    public enum DeviceStatus
    {
        Active,
        UnderMaintenance,
        Retired
    }

    public enum PrinterType
    {
        Laser,
        Inkjet
    }

    public interface INetworkable
    {
        string IpAddress { get; }
        void Connect(string ipAddress);
        void Disconnect();
        bool IsConnected { get; }
    }

    public abstract class Device
    {
        public string DeviceId { get; }
        public string DeviceName { get; set; }
        public int YearOfPuttingIntoUse { get; set; }
        public decimal PurchasePrice { get; set; }
        public DeviceStatus Status { get; set; }

        public Device(string deviceId, string deviceName, int year, decimal price, DeviceStatus status)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new ArgumentException("Mã thiết bị không được rỗng.");
            if (price <= 0)
                throw new ArgumentException("Giá mua phải lớn hơn 0.");
            if (year > DateTime.Now.Year)
                throw new ArgumentException("Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");

            DeviceId = deviceId;
            DeviceName = deviceName;
            YearOfPuttingIntoUse = year;
            PurchasePrice = price;
            Status = status;
        }

        // Phương thức trừu tượng tính chi phí bảo trì dự kiến
        public abstract decimal CalculateAnnualMaintenanceCost();

        public override string ToString()
        {
            return $"[{DeviceId}] {DeviceName} - Năm SD: {YearOfPuttingIntoUse}, Giá: {PurchasePrice:N0} VND, Trạng thái: {Status}";
        }
    }

    public class Computer : Device, INetworkable
    {
        public int RamCapacityGB { get; set; }
        public string ProcessorType { get; set; }
        public bool HasDiscreteGPU { get; set; }

        public string IpAddress { get; private set; }
        public bool IsConnected { get; private set; }

        public Computer(string id, string name, int year, decimal price, DeviceStatus status, int ram, string cpu, bool hasGPU)
            : base(id, name, year, price, status)
        {
            RamCapacityGB = ram;
            ProcessorType = cpu;
            HasDiscreteGPU = hasGPU;
            IpAddress = string.Empty;
            IsConnected = false;
        }

        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal cost = PurchasePrice * 0.05m;
            if (HasDiscreteGPU)
                cost += PurchasePrice * 0.02m;
            if (DateTime.Now.Year - YearOfPuttingIntoUse > 5)
                cost += PurchasePrice * 0.01m;
            return cost;
        }

        public void Connect(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("Địa chỉ IP không được rỗng.");
            if (IsConnected)
                throw new InvalidOperationException("Thiết bị đang kết nối mạng, không thể kết nối lại.");

            IpAddress = ipAddress;
            IsConnected = true;
        }

        public void Disconnect()
        {
            IpAddress = string.Empty;
            IsConnected = false;
        }

        public override string ToString()
        {
            string networkInfo = IsConnected ? $", IP: {IpAddress}" : ", Không có mạng";
            return base.ToString() + $" | Máy tính: {RamCapacityGB}GB RAM, {ProcessorType}, GPU rời: {HasDiscreteGPU}{networkInfo}";
        }
    }

    public class Printer : Device
    {
        public PrinterType Type { get; set; }
        public int PrintedPages { get; set; }
        public bool IsColorPrinter { get; set; }

        public Printer(string id, string name, int year, decimal price, DeviceStatus status, PrinterType type, int pages, bool isColor)
            : base(id, name, year, price, status)
        {
            Type = type;
            PrintedPages = pages;
            IsColorPrinter = isColor;
        }

        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal cost = PurchasePrice * 0.04m;
            if (PrintedPages > 100000)
                cost += 500000m;
            if (IsColorPrinter)
                cost += 300000m;
            return cost;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Máy in: {Type}, Đã in: {PrintedPages} trang, In màu: {IsColorPrinter}";
        }
    }

    public class NetworkPrinter : Printer, INetworkable
    {
        public string IpAddress { get; private set; }
        public bool IsConnected { get; private set; }

        public NetworkPrinter(string id, string name, int year, decimal price, DeviceStatus status, PrinterType type, int pages, bool isColor)
            : base(id, name, year, price, status, type, pages, isColor)
        {
            IpAddress = string.Empty;
            IsConnected = false;
        }

        public void Connect(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("Địa chỉ IP không được rỗng.");
            if (IsConnected)
                throw new InvalidOperationException("Thiết bị đang kết nối mạng, không thể kết nối lại.");

            IpAddress = ipAddress;
            IsConnected = true;
        }

        public void Disconnect()
        {
            IpAddress = string.Empty;
            IsConnected = false;
        }

        public override string ToString()
        {
            string networkInfo = IsConnected ? $", IP: {IpAddress}" : ", Không có mạng";
            return base.ToString() + $" {networkInfo} (Hỗ trợ mạng)";
        }
    }

    public class Projector : Device
    {
        public int BrightnessLumens { get; set; }
        public int BulbUsedHours { get; set; }

        public Projector(string id, string name, int year, decimal price, DeviceStatus status, int lumens, int bulbHours)
            : base(id, name, year, price, status)
        {
            BrightnessLumens = lumens;
            BulbUsedHours = bulbHours;
        }

        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal cost = PurchasePrice * 0.03m;
            if (BulbUsedHours > 3000)
                cost += 1500000m;
            return cost;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Máy chiếu: {BrightnessLumens} lumens, Số giờ bóng đèn: {BulbUsedHours}h";
        }
    }

    public class LabRoom
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
        private List<Device> Devices { get; set; }

        public LabRoom(string id, string name, int capacity)
        {
            RoomId = id;
            RoomName = name;
            Capacity = capacity;
            Devices = new List<Device>();
        }

        public void AddDevice(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "Không chấp nhận thiết bị null.");
            
            if (FindDevice(device.DeviceId) != null)
                throw new InvalidOperationException($"Thiết bị có mã {device.DeviceId} đã tồn tại trong phòng.");
            
            if (Devices.Count >= Capacity)
                throw new InvalidOperationException("Phòng đã đạt sức chứa tối đa.");

            Devices.Add(device);
        }

        public bool RemoveDevice(string deviceId)
        {
            var device = FindDevice(deviceId);
            if (device != null)
            {
                Devices.Remove(device);
                return true;
            }
            return false;
        }

        public Device FindDevice(string deviceId)
        {
            return Devices.Find(d => d.DeviceId == deviceId);
        }

        public decimal CalculateAnnualMaintenanceCost()
        {
            decimal totalCost = 0;
            foreach (var device in Devices)
            {
                totalCost += device.CalculateAnnualMaintenanceCost(); // Đa hình
            }
            return totalCost;
        }

        public List<Device> GetDevicesRequiringMaintenance()
        {
            int currentYear = DateTime.Now.Year;
            return Devices.FindAll(d => d.Status == DeviceStatus.UnderMaintenance || (currentYear - d.YearOfPuttingIntoUse > 5));
        }

        public IReadOnlyList<Device> GetAllDevices()
        {
            return Devices.AsReadOnly();
        }
    }
}
