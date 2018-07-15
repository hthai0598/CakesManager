using System;
using System.Collections.Generic;
using Persistence;
using DAL;
using System.Text.RegularExpressions;
using BL;
using System.Text;

namespace Console
{
    public class Program
    {
        static Staff staff;
        static StaffBL staffbl;
        static Item item;
        static ItemBL itembl;
        static InvoiceBL invoicebl;
        static Invoice invoice;
        static string u;
        static string pass;

        static void Main(string[] args)
        {
            MenuChoice();
        }
        private static short Menu(string title, string[] menuItems)
        {
            System.Console.Clear();
            short choose = 0;
            string line1 = "========================================";
            string line2 = "----------------------------------------";
            System.Console.WriteLine(line1);
            System.Console.WriteLine(" " + title);
            System.Console.WriteLine(line2);
            for (int i = 0; i < menuItems.Length; i++)
            {
                System.Console.WriteLine(" " + (i + 1) + ". " + menuItems[i]);
            }
            System.Console.WriteLine(line2);
            do
            {
                System.Console.Write("Bạn chọn: ");
                try
                {
                    choose = Int16.Parse(System.Console.ReadLine());

                }
                catch
                {
                    System.Console.WriteLine("Bạn chọn không đúng!");
                    continue;
                }
            }
            while (choose <= 0 || choose > menuItems.Length);
            return choose;
        }
        private static void MenuChoice()
        {
            System.Console.Clear();
            string[] choice = { "Đăng nhập", "Thoát chương trình" };
            int choose = Menu("Chào mừng bạn", choice);
            switch (choose)
            {
                case 1:
                    MenuLogin();
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
            }
        }
        public static void MenuLogin()
        {
            System.Console.Clear();
            string row1 = "==============================";
            string row2 = "-----------------------------------";
            System.Console.WriteLine(row1);
            System.Console.WriteLine(row2);
            System.Console.WriteLine("ĐĂNG NHẬP");
            System.Console.WriteLine("Nhập user name: ");
            u = System.Console.ReadLine();
            System.Console.WriteLine("Nhập Password: ");
            pass = Passwords();
            string choice;
            staffbl = new StaffBL();
            while (staffbl.Login(u, pass) == null || (Validate(u) == false) || (Validate(pass) == false))
            {
                System.Console.Clear();
                System.Console.Write("Đăng nhập lỗi, bạn có muốn tiếp tục đăng nhập không? (Y/N)");
                choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "Y":
                        break;
                    case "y":
                        break;
                    case "n":
                        MenuChoice();
                        break;
                    case "N":
                        MenuChoice();
                        break;
                    default:
                        continue;

                }
                System.Console.Clear();
                System.Console.WriteLine("Sai Username(Password)!");
                System.Console.WriteLine(row1);
                System.Console.WriteLine("ĐĂNG NHẬP");
                System.Console.WriteLine(row2);
                System.Console.Write("Nhập lại Username: ");
                u = System.Console.ReadLine();
                System.Console.Write("Nhập lại Password: ");
                pass = Passwords();
            }
            MenuStaff();


        }

        public static void MenuStaff()
        {
            staffbl = new StaffBL();
            staff = staffbl.Login(u, pass);
            System.Console.Clear();
            string[] staff1 = { "Xem Danh Sách Sản Phẩm", "Đăng Xuất" };

            // , "Tạo Hóa Đơn", "Xem Thống Kê"
            int chon = Menu("Chào Mừng Nhân Viên " + staff.StaffName + "Ca Làm việc : " + staff.calamviec, staff1);
            switch (chon)
            {
                // case 1:
                //     System.Console.WriteLine("Danh Sách Sản Phẩm ");
                //     List_Item();
                //     break;
                // case 2:
                //     // MenuThongKe();
                //     break;
                // case 3:

                //     break;
                case 1:
                System.Console.WriteLine("Xem Danh Sách Sản Phẩm");
                    List_Item();
                    break;
                
                case 3:
                    System.Console.WriteLine("Đăng Xuất ");
                    MenuChoice();

                    break;
            }

        }
        public static void List_Item()
        {
            System.Console.Clear();
            string line = "===========================================================================================================================================";
            System.Console.WriteLine(line);
            System.Console.WriteLine("Mã Sản Phẩm:  ||Tên Sản Phẩm           ||Loại Sản Phẩm ||Số Lượng   ||Giá          ||Size            || Khuyến Mãi      ");
            System.Console.WriteLine(line);
            itembl = new ItemBL();
            List<Item> it = itembl.GetAllItem();
            foreach (var item in it)
            {
                System.Console.WriteLine("{0,-15}{1,-25}{2,-15}{3,-15}{4,-15}{5,-15}{6,-15}", item.itemID, item.itemName, item.itemType, item.amount, item.unitPrice, item.size, item.Promotion);

            };
            System.Console.WriteLine("Nhấn Phím Bất Kì Để Bắt Đầu Mua:   ");
            System.Console.ReadKey();
            CreateOrders();
        }
        
        public static  bool CreateOrders()
        {
            System.Console.Clear();
            invoicebl=new InvoiceBL();
            invoice = new Invoice();
            itembl = new ItemBL();
            item = new Item();
            invoice.itemList = new List<Item>();
            int count1 = 0;
            invoice.Invoice_Status = 1;
            invoice.staff = new Staff();
            invoice.staff.StaffID = u;
            List<Item> result= itembl.GetAllItem();
            int index = 0;
            if( result != null)
            {
                while(true)
                {
                    while(true)
                    { 
                        int count = 0;
                        System.Console.WriteLine("Nhập Mã Sản Phẩm: ");
                        try{
                            
                            string item_id = System.Console.ReadLine();
                            for (int i = 0; i < result.Count; i++)
                            {
                                if(item_id == result[i].itemID)
                                {
                                    invoice.itemList.Add(itembl.GetItemById(item_id));
                                    index = i;
                                    count++;
                                }
                            
                                
                            }
                            if (count ==0)
                            {   
                                throw new System.Exception("Không tìm thấy ID");
                            }
                        }catch(System.Exception e)
                        {
                            System.Console.WriteLine(e.Message);
                            continue;
                        }
                        catch{
                            continue;
                        }
                        break;
                    }
                    
                        while (true)
                        {
                            try
                            {
                                    System.Console.Write("Nhập Vào Số Lượng: ");
                                    int amount = Convert.ToInt32(System.Console.ReadLine());
                                    if((amount >result[index].amount && result[index].amount == 0) || (amount == result[index].amount && result[index].amount == 0) )
                                    {
                                        System.Console.WriteLine("Số Lượng Sản Phẩm Không Còn!");
                                        System.Console.ReadKey();
                                        List_Item();
                                        
                                        }
                                    if( amount > result[index].amount && result[index].amount >0 )
                                    {
                                        System.Console.WriteLine("Số Lượng Còn : {0}",result[index].amount);
                                        throw (new System.Exception("Không Đủ, Nhập Lại: "));
                                    }
                                    else if( 0 < amount || amount <= result[index].amount)
                                    {
                                        invoice.itemList[count1].amount = amount;
                                        count1++;
                                        break;
                                    }
                            }
                            catch(System.Exception e)
                            {
                                System.Console.Write(e.Message);
                                continue;
                            }
                            catch
                            {
                                System.Console.Write("Hãy Nhập :");
                                continue;
                            }
                            break;
                        }
                        System.Console.Write("Bạn Muốn Mua Thêm  ?(y/n) ");
                        char choice = Convert.ToChar(System.Console.ReadLine());
                        if(choice == 'n')
                        {
                            break;
                        }
                
                    }
                System.Console.WriteLine("Create Order: " + (invoicebl.Create_Invoice(invoice) ? "Thành Công!" : "không Thành Công!"));
                System.Console.Write("  Nhấn Phím Bất Kì Để Xuất Hóa Đơn !");
                System.Console.ReadLine();
                
            }
            else if( result == null)
            {
                System.Console.Write("Sản Phẩm Không Tồn Tại!");
                System.Console.ReadLine();
                List_Item();
                return false;
            }
            return true;
            
        }























        private static bool Validate(string str)
        {
            Regex regex = new Regex("[a-zA-Z0-9_]");
            MatchCollection matchCollectionstr = regex.Matches(str);
            if (matchCollectionstr.Count < str.Length)
            {
                return false;
            }
            return true;
        }
        private static string Passwords()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = System.Console.ReadKey(true);
                if (cki.Key == System.ConsoleKey.Enter)
                {
                    System.Console.WriteLine();
                    break;
                }
                if (cki.Key == System.ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        System.Console.WriteLine("\b 0 \b");
                        sb.Length--;
                    }
                    continue;
                }

                System.Console.Write("*");
                sb.Append(cki.KeyChar);
            }
            return sb.ToString();
        }

    }
}
