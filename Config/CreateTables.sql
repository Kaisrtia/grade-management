-- Table Per Type (TPT) Implementation
-- Tạo bảng Users (bảng cha)

CREATE TABLE IF NOT EXISTS Users (
    Id VARCHAR(50) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(50) NOT NULL,
    Role ENUM('ADMIN', 'FMANAGER', 'STUDENT') NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tạo bảng Students (kế thừa từ Users)
CREATE TABLE IF NOT EXISTS Students (
    Id VARCHAR(50) PRIMARY KEY,
    Fid VARCHAR(50),
    FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    INDEX idx_fid (Fid)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tạo bảng FManagers (kế thừa từ Users)
CREATE TABLE IF NOT EXISTS FManagers (
    Id VARCHAR(50) PRIMARY KEY,
    Fid VARCHAR(50),
    FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    INDEX idx_fid (Fid)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tạo bảng Admins (kế thừa từ Users)
CREATE TABLE IF NOT EXISTS Admins (
    Id VARCHAR(50) PRIMARY KEY,
    FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Ví dụ: Insert dữ liệu
-- Khi thêm Student, cần thêm vào cả 2 bảng:
-- INSERT INTO Users (Id, Name, Username, Password, Role) VALUES ('S001', 'Nguyen Van A', 'student1', 'pass123', 'STUDENT');
-- INSERT INTO Students (Id, Fid) VALUES ('S001', 'F001');

-- Khi thêm FManager:
-- INSERT INTO Users (Id, Name, Username, Password, Role) VALUES ('F001', 'Tran Thi B', 'fmanager1', 'pass123', 'FMANAGER');
-- INSERT INTO FManagers (Id, Fid) VALUES ('F001', 'F001');

-- Khi thêm Admin:
-- INSERT INTO Users (Id, Name, Username, Password, Role) VALUES ('A001', 'Le Van C', 'admin1', 'pass123', 'ADMIN');
-- INSERT INTO Admins (Id) VALUES ('A001');
