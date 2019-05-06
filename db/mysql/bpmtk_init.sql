-- MySQL dump 10.13  Distrib 5.7.23, for Win64 (x86_64)
--
-- Host: localhost    Database: bpmtk3
-- ------------------------------------------------------
-- Server version	5.7.23-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bpm_act_data`
--

DROP TABLE IF EXISTS `bpm_act_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_act_data` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `type` varchar(128) NOT NULL,
  `byte_array_id` bigint(20) DEFAULT NULL,
  `text` longtext,
  `text2` longtext,
  `long_val` bigint(20) DEFAULT NULL,
  `double_val` double DEFAULT NULL,
  `act_inst_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_act_data_act_inst_id` (`act_inst_id`),
  KEY `IX_bpm_act_data_byte_array_id` (`byte_array_id`),
  CONSTRAINT `FK_bpm_act_data_bpm_act_inst_act_inst_id` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_act_data_bpm_byte_array_byte_array_id` FOREIGN KEY (`byte_array_id`) REFERENCES `bpm_byte_array` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_act_inst`
--

DROP TABLE IF EXISTS `bpm_act_inst`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_act_inst` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `state` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `created` datetime(6) NOT NULL,
  `start_time` datetime(6) DEFAULT NULL,
  `last_state_time` datetime(6) NOT NULL,
  `description` longtext,
  `concurrency_stamp` varchar(50) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `sub_proc_inst_id` bigint(20) DEFAULT NULL,
  `activity_id` varchar(64) NOT NULL,
  `activity_type` varchar(16) NOT NULL,
  `is_mi_root` bit(1) NOT NULL,
  `token_id` bigint(20) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_act_inst_parent_id` (`parent_id`),
  KEY `IX_bpm_act_inst_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_act_inst_sub_proc_inst_id` (`sub_proc_inst_id`),
  CONSTRAINT `FK_bpm_act_inst_bpm_act_inst_parent_id` FOREIGN KEY (`parent_id`) REFERENCES `bpm_act_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_act_inst_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_bpm_act_inst_bpm_proc_inst_sub_proc_inst_id` FOREIGN KEY (`sub_proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=467 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_byte_array`
--

DROP TABLE IF EXISTS `bpm_byte_array`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_byte_array` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `value` longblob,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_comment`
--

DROP TABLE IF EXISTS `bpm_comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_comment` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(32) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `body` varchar(512) NOT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `task_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_comment_proc_def_id` (`proc_def_id`),
  KEY `IX_bpm_comment_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_comment_task_id` (`task_id`),
  CONSTRAINT `FK_bpm_comment_bpm_proc_def_proc_def_id` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_comment_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_comment_bpm_task_task_id` FOREIGN KEY (`task_id`) REFERENCES `bpm_task` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_deployment`
--

DROP TABLE IF EXISTS `bpm_deployment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_deployment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `tenant_id` longtext,
  `category` varchar(64) DEFAULT NULL,
  `model_id` bigint(20) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `user_id` varchar(32) DEFAULT NULL,
  `package_id` int(11) DEFAULT NULL,
  `memo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_deployment_model_id` (`model_id`),
  KEY `IX_bpm_deployment_package_id` (`package_id`),
  CONSTRAINT `FK_bpm_deployment_bpm_byte_array_model_id` FOREIGN KEY (`model_id`) REFERENCES `bpm_byte_array` (`id`),
  CONSTRAINT `FK_bpm_deployment_bpm_package_package_id` FOREIGN KEY (`package_id`) REFERENCES `bpm_package` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_event_subscr`
--

DROP TABLE IF EXISTS `bpm_event_subscr`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_event_subscr` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `event_type` varchar(50) NOT NULL,
  `event_name` varchar(50) NOT NULL,
  `activity_id` longtext,
  `proc_def_id` int(11) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `tenant_id` longtext,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_event_subscr_proc_def_id` (`proc_def_id`),
  KEY `IX_bpm_event_subscr_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_event_subscr_token_id` (`token_id`),
  CONSTRAINT `FK_bpm_event_subscr_bpm_proc_def_proc_def_id` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`),
  CONSTRAINT `FK_bpm_event_subscr_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_bpm_event_subscr_bpm_token_token_id` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_group`
--

DROP TABLE IF EXISTS `bpm_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_group` (
  `id` varchar(255) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_identity_link`
--

DROP TABLE IF EXISTS `bpm_identity_link`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_identity_link` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `user_id` longtext,
  `group_id` longtext,
  `type` varchar(50) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `task_id` bigint(20) DEFAULT NULL,
  `act_inst_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_identity_link_act_inst_id` (`act_inst_id`),
  KEY `IX_bpm_identity_link_proc_def_id` (`proc_def_id`),
  KEY `IX_bpm_identity_link_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_identity_link_task_id` (`task_id`),
  CONSTRAINT `FK_bpm_identity_link_bpm_act_inst_act_inst_id` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_identity_link_bpm_proc_def_proc_def_id` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_identity_link_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_identity_link_bpm_task_task_id` FOREIGN KEY (`task_id`) REFERENCES `bpm_task` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_package`
--

DROP TABLE IF EXISTS `bpm_package`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_package` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tenant_id` longtext,
  `category` longtext,
  `owner_id` longtext,
  `name` longtext,
  `description` longtext,
  `version` int(11) NOT NULL,
  `concurrency_stamp` longtext,
  `created` datetime(6) NOT NULL,
  `modified` datetime(6) NOT NULL,
  `source_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_package_source_id` (`source_id`),
  CONSTRAINT `FK_bpm_package_bpm_byte_array_source_id` FOREIGN KEY (`source_id`) REFERENCES `bpm_byte_array` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_proc_data`
--

DROP TABLE IF EXISTS `bpm_proc_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_proc_data` (
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `type` varchar(128) NOT NULL,
  `byte_array_id` bigint(20) DEFAULT NULL,
  `text` longtext,
  `text2` longtext,
  `long_val` bigint(20) DEFAULT NULL,
  `double_val` double DEFAULT NULL,
  `task_id` bigint(20) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_proc_data_byte_array_id` (`byte_array_id`),
  KEY `IX_bpm_proc_data_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_proc_data_task_id` (`task_id`),
  KEY `IX_bpm_proc_data_token_id` (`token_id`),
  CONSTRAINT `FK_bpm_proc_data_bpm_byte_array_byte_array_id` FOREIGN KEY (`byte_array_id`) REFERENCES `bpm_byte_array` (`id`),
  CONSTRAINT `FK_bpm_proc_data_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_proc_data_bpm_task_task_id` FOREIGN KEY (`task_id`) REFERENCES `bpm_task` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_proc_data_bpm_token_token_id` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_proc_def`
--

DROP TABLE IF EXISTS `bpm_proc_def`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_proc_def` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `category` varchar(50) DEFAULT NULL,
  `deployment_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `key` varchar(64) NOT NULL,
  `version` int(11) NOT NULL,
  `created` datetime(6) NOT NULL,
  `modified` datetime(6) NOT NULL,
  `has_diagram` bit(1) NOT NULL,
  `valid_from` datetime(6) DEFAULT NULL,
  `valid_to` datetime(6) DEFAULT NULL,
  `concurrency_stamp` longtext,
  `state` int(11) NOT NULL,
  `version_tag` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_bpm_proc_def_key_version` (`key`,`version`),
  KEY `IX_bpm_proc_def_deployment_id` (`deployment_id`),
  CONSTRAINT `FK_bpm_proc_def_bpm_deployment_deployment_id` FOREIGN KEY (`deployment_id`) REFERENCES `bpm_deployment` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_proc_inst`
--

DROP TABLE IF EXISTS `bpm_proc_inst`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_proc_inst` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `state` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `created` datetime(6) NOT NULL,
  `start_time` datetime(6) DEFAULT NULL,
  `last_state_time` datetime(6) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `concurrency_stamp` longtext,
  `tenant_id` longtext,
  `key` varchar(32) DEFAULT NULL,
  `initiator` varchar(32) DEFAULT NULL,
  `end_reason` varchar(255) DEFAULT NULL,
  `super_id` bigint(20) DEFAULT NULL,
  `proc_def_id` int(11) NOT NULL,
  `caller_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_proc_inst_caller_id` (`caller_id`),
  KEY `IX_bpm_proc_inst_proc_def_id` (`proc_def_id`),
  KEY `IX_bpm_proc_inst_super_id` (`super_id`),
  CONSTRAINT `FK_bpm_proc_inst_bpm_act_inst_caller_id` FOREIGN KEY (`caller_id`) REFERENCES `bpm_act_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_proc_inst_bpm_proc_def_proc_def_id` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`),
  CONSTRAINT `FK_bpm_proc_inst_bpm_token_super_id` FOREIGN KEY (`super_id`) REFERENCES `bpm_token` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_scheduled_job`
--

DROP TABLE IF EXISTS `bpm_scheduled_job`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_scheduled_job` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `key` longtext,
  `retries` int(11) NOT NULL,
  `type` varchar(50) NOT NULL,
  `handler` longtext,
  `due_date` datetime(6) DEFAULT NULL,
  `end_date` datetime(6) DEFAULT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `activity_id` longtext,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `message` longtext,
  `stack_trace` longtext,
  `token_id` bigint(20) DEFAULT NULL,
  `tenant_id` longtext,
  `created` datetime(6) NOT NULL,
  `options` longtext,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_scheduled_job_proc_def_id` (`proc_def_id`),
  KEY `IX_bpm_scheduled_job_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_scheduled_job_token_id` (`token_id`),
  CONSTRAINT `FK_bpm_scheduled_job_bpm_proc_def_proc_def_id` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`),
  CONSTRAINT `FK_bpm_scheduled_job_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_bpm_scheduled_job_bpm_token_token_id` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_task`
--

DROP TABLE IF EXISTS `bpm_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `act_inst_id` bigint(20) DEFAULT NULL,
  `state` int(11) NOT NULL,
  `last_state_time` datetime(6) NOT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  `name` varchar(100) NOT NULL,
  `priority` smallint(6) NOT NULL,
  `activity_id` varchar(64) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `claimed_time` datetime(6) DEFAULT NULL,
  `assignee` varchar(32) DEFAULT NULL,
  `due_date` datetime(6) DEFAULT NULL,
  `modified` datetime(6) NOT NULL,
  `concurrency_stamp` longtext,
  `description` longtext,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_task_act_inst_id` (`act_inst_id`),
  KEY `IX_bpm_task_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_task_token_id` (`token_id`),
  CONSTRAINT `FK_bpm_task_bpm_act_inst_act_inst_id` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_task_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_task_bpm_token_token_id` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=109 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_token`
--

DROP TABLE IF EXISTS `bpm_token`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_token` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `activity_id` varchar(64) DEFAULT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `is_scope` bit(1) NOT NULL,
  `is_active` bit(1) NOT NULL,
  `act_inst_id` bigint(20) DEFAULT NULL,
  `proc_inst_id` bigint(20) NOT NULL,
  `sub_proc_inst_id` bigint(20) DEFAULT NULL,
  `transition_id` varchar(64) DEFAULT NULL,
  `is_suspended` bit(1) NOT NULL,
  `is_mi_root` bit(1) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_bpm_token_act_inst_id` (`act_inst_id`),
  KEY `IX_bpm_token_parent_id` (`parent_id`),
  KEY `IX_bpm_token_proc_inst_id` (`proc_inst_id`),
  KEY `IX_bpm_token_sub_proc_inst_id` (`sub_proc_inst_id`),
  CONSTRAINT `FK_bpm_token_bpm_act_inst_act_inst_id` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`) ON DELETE SET NULL,
  CONSTRAINT `FK_bpm_token_bpm_proc_inst_proc_inst_id` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_token_bpm_proc_inst_sub_proc_inst_id` FOREIGN KEY (`sub_proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_bpm_token_bpm_token_parent_id` FOREIGN KEY (`parent_id`) REFERENCES `bpm_token` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_user`
--

DROP TABLE IF EXISTS `bpm_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_user` (
  `id` varchar(255) NOT NULL,
  `name` longtext,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bpm_user_group`
--

DROP TABLE IF EXISTS `bpm_user_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bpm_user_group` (
  `user_id` varchar(255) NOT NULL,
  `group_id` varchar(255) NOT NULL,
  PRIMARY KEY (`user_id`,`group_id`),
  KEY `IX_bpm_user_group_group_id` (`group_id`),
  CONSTRAINT `FK_bpm_user_group_bpm_group_group_id` FOREIGN KEY (`group_id`) REFERENCES `bpm_group` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_bpm_user_group_bpm_user_user_id` FOREIGN KEY (`user_id`) REFERENCES `bpm_user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-05-06 18:18:49
