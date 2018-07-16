create database CakeManager;
drop database cakemanager;
use CakeManager;
create table Item
(
	ItemID char(40) primary key,
    ItemName char(40),
    UnitPrice decimal,
    ItemType char(40),
    Size char(40),
    Amount int,
    Promotion char(40)
);
insert into Item(ItemID,ItemName,UnitPrice,ItemType,Size,Amount,Promotion) values
 ('GT1','Mousse Chanh Leo',250.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT2','Mousse Hawaii',250.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT3','MOUSSE XOÀI',250.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT4','Sour Cream ',200.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT5','Sour Cream ',200.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT6','Bánh Mousse Ý',300.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT7','Flan bong',270.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT8','Flan tra xanh',250.000,'GATO','35x25cm',5,'Nến,Dao,Nĩa'),
 ('GT9','Flan socola',250.000,'GATO','15x25cm',5,'Nến,Dao,Nĩa'),
 ('GT10','Sour Cream Me',250.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT11','Sour Raspberry ',250.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT12','Sour Cheesecake',250.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT13','Gato Kem Bo Opera ',450.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT14','Gato Kem Bo',400.000,'GATO','30x25cm',5,'Nến,Dao,Nĩa'),
 ('GT15','Gato capuchino',350.000,'GATO','25x25cm',5,'Nến,Dao,Nĩa'),
 ('GT16','Gato Kem Tuoi',350.000,'GATO','35x25cm',5,'Nến,Dao,Nĩa'),
 ('GT17','Gato Tiramisu',350.000,'GATO','27x27cm',5,'Nến,Dao,Nĩa'),
 ('CK1','Cookies Luỡi',50.000,'Cookies','Không',5,'Không'),
 ('CK2','Bánh Vòng',50.000,'Cookies','Không',5,'Không'),
 ('CK3','Cookies Nhân',52.000,'Cookies','Không',5,'Không'),
 ('CK4','Cookies Tuille',50.000,'Cookies','Không',5,'Không'),
 ('CK5','Bánh Quy Macaron',55.000,'Cookies','Không',5,'Không'),
 ('CK6','Cookies Dừa ',45.000,'Cookies','Không',5,'Không'),
 ('CK7','Cookies Luỡi Mèo',60.000,'Cookies','Không',5,'Không'),
 ('BM1','Bánh Mì Chuột',2.000,'Bánh Mì','Không',100,'Không'),
 ('BM2','Mini Pizza',45.000,'Bánh Mì','Không',100,'Không'),
 ('BM3','Bánh Mì Bo Ðường',20.000,'Bánh Mì','Không',100,'Không'),
 ('BM4','Vỏ Humberger',42.000,'Bánh Mì','Không',100,'Không'),
 ('BM5','Bánh Mi Xúc Xích',42.000,'Bánh Mì','Không',100,'Không'),
 ('BM6','Bánh Mì Sữa Pháp',29.000,'Bánh Mì','Không',100,'Không'),
 ('BM7','Bánh Mì Gậy',5.000,'Bánh Mì','Không',100,'Không'),
 ('BM8','Bánh Mì Gối Bo',40.000,'Bánh Mì','Không',100,'Không'),
 ('BM9','Bánh Mì Gối Nho',50.000,'Bánh Mì','Không',100,'Không'),
 ('BM10','Bánh Mì Gối Thường',20.000,'Bánh Mì','Không',100,'Không'),
 ('BM11','Bánh Mì Ðen',55.000,'Bánh Mì','Không',100,'Không'),
 ('BM12','Bánh Mì Ngu Cốc',55.000,'Bánh Mì','Không',100,'Không'),
 ('BM13','Bánh Mì Gối Ðen',55.000,'Bánh Mì','Không',100,'Không'),
 ('BM14','Bánh Mì Gối Bẹp',42.000,'Bánh Mì','Không',100,'Không'),
 ('BM15','Bánh Mì Sữa Hokkaido',52.000,'Bánh Mì','Không',100,'Không');


create table Staff
(
	StaffID char(40) primary key,
    StaffName char(40),
    StaffPass char(40),
    Calamviec char(40)
);
insert into Staff(StaffID, StaffName,StaffPass,Calamviec) values
 ('S111','Nguyễn Văn A','123','Sáng'),
 ('C111','Nguyễn Văn B','456','Chiều');

create table Invoice
(
	InvoiceID int not null primary key auto_increment,
    StaffID char(40),
    InvoiceDate datetime default now(),
    InvoiceStatus int,
    constraint fk_Invoice_Staff foreign key (StaffID) references Staff(StaffID)
);

insert into Invoice(StaffID,InvoiceStatus) value ('S111',2);

create table InvoiceDetails
(
	ItemID char(40),
    InvoiceID int ,
    Amount int,
    InvoiceStatus int,
    UnitPrice decimal,
    constraint fk_InvoiceDetails_Item foreign key (ItemID) references Item(ItemID),
    constraint fk_InvoiceDetails_Invoice foreign key (InvoiceID) references Invoice(InvoiceID)
);
select * from invoicedetails;

insert into invoicedetails(ItemID,InvoiceID,Amount,InvoiceStatus,UnitPrice) value ('GT1',8,4,2,20);

select staff.StaffID,staff.StaffName, invoice.InvoiceID,invoicedetails.Amount,invoicedetails.UnitPrice,item.ItemName,item.ItemID,item.UnitPrice from staff inner join invoice on staff.StaffID = invoice.StaffID inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID inner join item on item.ItemID = invoicedetails.ItemID where invoice.InvoiceID = 8;
