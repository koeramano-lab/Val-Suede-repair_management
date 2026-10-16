-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 16, 2026 at 05:56 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `repair_management`
--

-- --------------------------------------------------------

--
-- Table structure for table `repair_requests`
--

CREATE TABLE `repair_requests` (
  `repair_id` int(11) NOT NULL,
  `reporter_name` varchar(100) NOT NULL,
  `department` varchar(100) NOT NULL,
  `equipment_type` varchar(100) NOT NULL,
  `problem_details` text NOT NULL,
  `report_date` date NOT NULL,
  `repair_status` enum('รอดำเนินการ','กำลังซ่อม','ซ่อมเสร็จแล้ว') NOT NULL DEFAULT 'รอดำเนินการ'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `repair_requests`
--

INSERT INTO `repair_requests` (`repair_id`, `reporter_name`, `department`, `equipment_type`, `problem_details`, `report_date`, `repair_status`) VALUES
(1, 'ธร', 'Mat', 'คอมพิวเตอร์ตั้งโต๊ะ', 'หน้าจอเปิดไม่ติด', '2026-10-14', 'ซ่อมเสร็จแล้ว'),
(2, 'นา', 'จัดซื้อ', 'เครื่องพิมพ์', 'หมึกหมด', '2026-10-16', 'กำลังซ่อม'),
(5, 'มาวิน', 'ฝ่ายขาย', 'อุปกรณ์เครือข่าย', 'สาย LAN ขาด', '2026-10-16', 'รอดำเนินการ');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `repair_requests`
--
ALTER TABLE `repair_requests`
  ADD PRIMARY KEY (`repair_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `repair_requests`
--
ALTER TABLE `repair_requests`
  MODIFY `repair_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
