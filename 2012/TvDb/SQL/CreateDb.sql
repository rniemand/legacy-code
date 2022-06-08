# --------------------------------------------------------
# Host:                         192.168.0.5
# Server version:               5.5.16
# Server OS:                    Win32
# HeidiSQL version:             6.0.0.3603
# Date/time:                    2012-04-27 13:53:57
# --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

# Dumping structure for table tvdb.tb_series
CREATE TABLE IF NOT EXISTS `tb_series` (
  `_id` int(16) NOT NULL AUTO_INCREMENT,
  `seriesId` int(16) NOT NULL DEFAULT '0',
  `id` int(16) NOT NULL DEFAULT '0',
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `dateUpdated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `language` varchar(4) DEFAULT 'en',
  `seriesName` varchar(128) DEFAULT NULL,
  `imdbId` varchar(128) DEFAULT NULL,
  `zap2itId` varchar(128) DEFAULT NULL,
  `banner` varchar(256) DEFAULT NULL,
  `overview` text,
  PRIMARY KEY (`_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

# Data exporting was unselected.
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
