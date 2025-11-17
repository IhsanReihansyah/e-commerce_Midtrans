-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 17, 2025 at 09:31 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `eshop`
--

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `Id` int(11) NOT NULL,
  `OrderId` longtext NOT NULL,
  `Amount` decimal(65,30) NOT NULL,
  `PaymentStatus` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CustomerAddress` longtext NOT NULL,
  `CustomerEmail` longtext NOT NULL,
  `CustomerName` longtext NOT NULL,
  `CustomerPhone` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`Id`, `OrderId`, `Amount`, `PaymentStatus`, `CreatedAt`, `CustomerAddress`, `CustomerEmail`, `CustomerName`, `CustomerPhone`) VALUES
(13, '67dae5e9-84fc-476b-a44f-79646cab9a4f', 860000.000000000000000000000000000000, 'pending', '2025-11-14 14:18:52.069454', 'rehan', 'rehan@gmail.com', 'rehan', 'rehan'),
(14, 'e6eff006-0c9f-4007-a265-fbf4446a24d5', 45000.000000000000000000000000000000, 'pending', '2025-11-14 15:03:42.577048', 're', 'rehan@gmail.com', 'ewe', 'rehan');

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `Price` decimal(65,30) NOT NULL,
  `ImageUrl` longtext NOT NULL,
  `DeletedAt` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`Id`, `Name`, `Price`, `ImageUrl`, `DeletedAt`) VALUES
(2, 'Oversize T-Shirt', 149000.000000000000000000000000000000, 'https://www.miraclemates.id/cdn/shop/files/Basic3.jpg?v=1698117328&width=493', NULL),
(4, 'Mountain Hat', 45000.000000000000000000000000000000, 'https://cozmeed.com/wp-content/uploads/2019/08/2.jpg', NULL),
(5, 'Cargo Pants', 215000.000000000000000000000000000000, 'https://cozmeed.com/wp-content/uploads/2024/12/1-1.jpg', NULL),
(6, 'Mountain Jacket', 312000.000000000000000000000000000000, 'https://cozmeed.com/wp-content/uploads/2024/12/1733469963990_1-2.jpeg', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20251112085018_InitialCreate', '9.0.0'),
('20251112114724_AddOrdersTable', '9.0.0'),
('20251113063908_AddOrderTable', '9.0.0'),
('20251113074922_CreateOrdersTable', '9.0.0'),
('20251113080745_AddCustomerFieldsToOrder', '9.0.0');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
