﻿CREATE DATABASE book_store
GO

USE book_store
GO

CREATE TABLE [account] (
  [account_id] integer PRIMARY KEY IDENTITY(1,1),
  [username] nvarchar(255),
  [email] nvarchar(255),
  [password] nvarchar(255),
  [role] nvarchar(255),
  [created_at] datetime
)
GO

CREATE TABLE [book] (
  [book_id] integer PRIMARY KEY IDENTITY(1,1),
  [author_id] integer,
  [category_id] integer,
  [name] nvarchar(255),
  [image] nvarchar(255),
  [description] nvarchar(MAX),
  [publish_company] nvarchar(255),
  [publish_year] int,
  [price] int,
  [sold] int,
  [remain] int,
  [is_deleted] int,
  [created_at] datetime
)
GO

CREATE TABLE [category] (
  [category_id] integer PRIMARY KEY IDENTITY(1,1),
  [name] nvarchar(255),
  [description] nvarchar(MAX),
  [created_at] datetime
)
GO

CREATE TABLE [author] (
  [author_id] integer PRIMARY KEY IDENTITY(1,1),
  [name] nvarchar(255),
  [image] nvarchar(255),
  [description] nvarchar(MAX),
  [is_deleted] int,
  [created_at] datetime
)
GO

CREATE TABLE [cart] (
  [cart_id] integer PRIMARY KEY IDENTITY(1,1),
  [account_id] integer
)
GO

CREATE TABLE [cart_book] (
  [card_id] integer,
  [book_id] integer,
  [quantity] int,
  [status] int,
  [total_amount] int,
  PRIMARY KEY ([card_id], [book_id])
)
GO

CREATE TABLE [order] (
  [order_id] integer PRIMARY KEY IDENTITY(1,1),
  [account_id] integer,
  [created_at] datetime
)
GO

CREATE TABLE [order_book] (
  [order_id] integer,
  [book_id] integer,
  [quantity] int,
  [total_amount] int,
  PRIMARY KEY ([order_id], [book_id])
)
GO

ALTER TABLE [book] ADD FOREIGN KEY ([author_id]) REFERENCES [author] ([author_id])
GO

ALTER TABLE [book] ADD FOREIGN KEY ([category_id]) REFERENCES [category] ([category_id])
GO

ALTER TABLE [order_book] ADD FOREIGN KEY ([book_id]) REFERENCES [book] ([book_id])
GO

ALTER TABLE [cart_book] ADD FOREIGN KEY ([card_id]) REFERENCES [book] ([book_id])
GO

ALTER TABLE [order_book] ADD FOREIGN KEY ([order_id]) REFERENCES [order] ([order_id])
GO

ALTER TABLE [cart_book] ADD FOREIGN KEY ([card_id]) REFERENCES [cart] ([cart_id])
GO

ALTER TABLE [order] ADD FOREIGN KEY ([account_id]) REFERENCES [account] ([account_id])
GO

ALTER TABLE [cart] ADD FOREIGN KEY ([account_id]) REFERENCES [account] ([account_id])
GO

INSERT INTO account(username, email, password, role, created_at) VALUES
(N'Nguyễn Duy Minh Quân', 'ndmquan1010@gmail.com', 'quanduy1010', 'client', '2004-05-23T14:25:10');

INSERT INTO author(name, image, description, is_deleted) VALUES
(N'Chiziwa Maresuke', N'gabriel-garcia-marquez.jpg', N'Tác giả nổi tiếng của Nhật.', 1),
(N'Saionji Iwao', N'victor-hugo.jpg', N'Tác giả nổi tiếng của Nhật.', 1),
(N'Nobusawa Tokutomi', N'nam-cao.jpg', N'Tác giả nổi tiếng của Nhật.', 1),
(N'Haruki Murakami', N'haruki-murakami.jpg', N'Tác giả nổi tiếng của Nhật.', 1);

INSERT INTO category(name, description, created_at) VALUES
(N'Shounen', N'Truyện cho con trai', '2004-05-23T14:25:10'), 
(N'Shoujo', N'Truyện cho con gái', '2004-05-23T14:25:10'),
(N'Isekai', N'Truyện xuyên không, chuyển sinh', '2004-05-23T14:25:10'),
(N'Horror', N'Truyện kinh dị', '2004-05-23T14:25:10');

INSERT INTO book(author_id, category_id, name, image, description, publish_company, publish_year, price, sold, remain, is_deleted) VALUES
(1, 4, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(2, 3, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(3, 2, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(4, 1, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(1, 1, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(2, 2, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(3, 3, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1),
(4, 4, N'Maou 2099', 'maou_2099.jpg', N'Năm 2099 tại thành phố Shinjuku, Ma Vương huyền thoại Veltol đã trở lại thành phố tương lai đã phát triển thịnh vượng. Đi kèm với hào quang rực rỡ của quốc gia đô thị bậc nhất... là "bóng tối" ghê tởm ẩn chứa đằng sau. Để thống trị thế giới mới, Ma Vương đã dấn thân vào con đường đầy chông gai.', N'Nhã Nam', 2021, 120000, 321, 140, 1);

INSERT INTO book(author_id, category_id, name, image, description, publish_company, publish_year, price, sold, remain, is_deleted) VALUES
(1, 4, N'Rekkyou Sensen', 'rekkyou-sensen_1712122099.jpg',N'Nhân loại được trí tuệ nhân tạo Gaia cho biết rằng con người là nguyên nhân của việc này, và để giảm bớt số lượng con người, họ bắt đầu một cuộc chiến để quyết định quốc gia nào sẽ bị diệt vong.' , N'Ánh Dương', 2021, 200000, 300, 200, 1),
(2, 3, N'Umi No Mukou Kara Kita Otoko', 'nguoi-dan-ong-den-tu-ben-kia-dai-duong_1702262029.jpg',  N'Lấy bối cảnh ở một thị trấn cảng ở Tohoku hướng ra biển với những hòn đảo xinh đẹp rải rác. Minami lớn lên ở đó và làm việc cho ông nội anh, Masakichi, người điều hành một con thuyền đánh cá. Kể từ khi cha anh đột ngột biến mất, Masakichi đã đóng vai trò như là cha của anh. Vào sinh nhật thứ 20 của Minami, một người đàn ông bí ẩn xuất hiện từ bên kia đại dương, trông giống anh y như đúc...!? Danh tính thực sự của gã là gì?', N'NXB Trẻ', 2023, 100000, 400, 140, 1),
(3, 2, N'Thị Trấn Quái Dị - Soil', 'thi-tran-quai-di-soil_1691834386.jpg',  N'Có điều gì đó kỳ lạ đang diễn ra ở "Soil New Town". Sự biến mất đột ngột của một gia đình tưởng như bình thường đã đưa hai thám tử Yokoi và Onoda đến một thị trấn bình dị giữa hư không. Ban đầu trông giống như một trường hợp thông thường, song nhanh chóng biến thành một câu đố phức tạp và chết người mà không có gì giống như vẻ ngoài của nó. Liệu hai nhà điều tra tuyệt vọng có giải quyết được bí ẩn trước khi quá muộn? ',N'Ánh Dương', 2022, 150000, 350, 140, 1)

