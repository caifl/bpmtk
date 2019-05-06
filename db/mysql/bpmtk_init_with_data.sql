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
-- Dumping data for table `bpm_act_data`
--

LOCK TABLES `bpm_act_data` WRITE;
/*!40000 ALTER TABLE `bpm_act_data` DISABLE KEYS */;
INSERT INTO `bpm_act_data` VALUES (1,'numberOfInstances','int',NULL,NULL,NULL,5,NULL,54),(2,'numberOfCompletedInstances','int',NULL,NULL,NULL,5,NULL,54),(3,'numberOfActiveInstances','int',NULL,NULL,NULL,0,NULL,54),(4,'loopCounter','int',NULL,NULL,NULL,0,NULL,55),(5,'loopCounter','int',NULL,NULL,NULL,1,NULL,56),(6,'loopCounter','int',NULL,NULL,NULL,2,NULL,57),(7,'loopCounter','int',NULL,NULL,NULL,3,NULL,58),(8,'loopCounter','int',NULL,NULL,NULL,4,NULL,59),(101,'numberOfInstances','int',NULL,NULL,NULL,4,NULL,418),(102,'numberOfCompletedInstances','int',NULL,NULL,NULL,4,NULL,418),(103,'numberOfActiveInstances','int',NULL,NULL,NULL,0,NULL,418),(104,'loopCounter','int',NULL,NULL,NULL,0,NULL,419),(105,'loopCounter','int',NULL,NULL,NULL,1,NULL,427),(106,'loopCounter','int',NULL,NULL,NULL,2,NULL,435),(107,'loopCounter','int',NULL,NULL,NULL,3,NULL,443),(108,'numberOfInstances','int',NULL,NULL,NULL,5,NULL,453),(109,'numberOfCompletedInstances','int',NULL,NULL,NULL,5,NULL,453),(110,'numberOfActiveInstances','int',NULL,NULL,NULL,0,NULL,453),(111,'loopCounter','int',NULL,NULL,NULL,0,NULL,454),(112,'loopCounter','int',NULL,NULL,NULL,1,NULL,455),(113,'loopCounter','int',NULL,NULL,NULL,2,NULL,456),(114,'loopCounter','int',NULL,NULL,NULL,3,NULL,457),(115,'loopCounter','int',NULL,NULL,NULL,4,NULL,458);
/*!40000 ALTER TABLE `bpm_act_data` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_act_inst`
--

LOCK TABLES `bpm_act_inst` WRITE;
/*!40000 ALTER TABLE `bpm_act_inst` DISABLE KEYS */;
INSERT INTO `bpm_act_inst` VALUES (3,4,'Start','2019-05-06 12:49:08.909394','2019-05-06 12:49:08.933638','2019-05-06 12:49:08.944394',NULL,NULL,4,NULL,NULL,'StartEvent_1846cge','StartEvent',_binary '\0',2),(4,4,'Task 0','2019-05-06 12:49:08.947030','2019-05-06 12:49:08.951451','2019-05-06 12:49:09.159984',NULL,NULL,4,NULL,NULL,'Task_0p1lhmm','UserTask',_binary '\0',2),(5,4,'Fork_AB','2019-05-06 12:49:09.161148','2019-05-06 12:49:09.163776','2019-05-06 12:49:09.168431',NULL,NULL,4,NULL,NULL,'ExclusiveGateway_07emx2k','ParallelGateway',_binary '\0',2),(6,4,'Task A','2019-05-06 12:49:09.175415','2019-05-06 12:49:09.178131','2019-05-06 12:49:09.200200',NULL,NULL,4,NULL,NULL,'Task_0gnw4s9','UserTask',_binary '\0',3),(7,4,'Task B','2019-05-06 12:49:09.183741','2019-05-06 12:49:09.186423','2019-05-06 12:49:09.211862',NULL,NULL,4,NULL,NULL,'Task_0dtq8u2','UserTask',_binary '\0',4),(8,4,'Join_AB','2019-05-06 12:49:09.201829','2019-05-06 12:49:09.292856','2019-05-06 12:49:09.298205',NULL,NULL,4,NULL,NULL,'ExclusiveGateway_082abmy','ParallelGateway',_binary '\0',3),(9,4,'Fork_B','2019-05-06 12:49:09.214063','2019-05-06 12:49:09.216575','2019-05-06 12:49:09.218978',NULL,NULL,4,NULL,NULL,'ExclusiveGateway_0svb4en','ParallelGateway',_binary '\0',4),(10,4,'Task B1','2019-05-06 12:49:09.224491','2019-05-06 12:49:09.227240','2019-05-06 12:49:09.244684',NULL,NULL,4,NULL,NULL,'Task_1mcwerv','UserTask',_binary '\0',5),(11,4,'Task B2','2019-05-06 12:49:09.232081','2019-05-06 12:49:09.235293','2019-05-06 12:49:09.253553',NULL,NULL,4,NULL,NULL,'Task_000icim','UserTask',_binary '\0',6),(12,4,'Join_B','2019-05-06 12:49:09.247231','2019-05-06 12:49:09.259472','2019-05-06 12:49:09.287892',NULL,NULL,4,NULL,NULL,'ExclusiveGateway_08wqw3l','ParallelGateway',_binary '\0',5),(13,4,'Join_B','2019-05-06 12:49:09.256287','2019-05-06 12:49:09.259472','2019-05-06 12:49:09.287892',NULL,NULL,4,NULL,NULL,'ExclusiveGateway_08wqw3l','ParallelGateway',_binary '\0',6),(14,4,'Join_AB','2019-05-06 12:49:09.290050','2019-05-06 12:49:09.292856','2019-05-06 12:49:09.298205',NULL,NULL,4,NULL,NULL,'ExclusiveGateway_082abmy','ParallelGateway',_binary '\0',2),(15,4,'Task C','2019-05-06 12:49:09.301176','2019-05-06 12:49:09.305205','2019-05-06 12:49:09.322625',NULL,NULL,4,NULL,NULL,'Task_0k5jzhg','UserTask',_binary '\0',2),(16,4,'End','2019-05-06 12:49:09.325497','2019-05-06 12:49:09.328523','2019-05-06 12:49:09.332726',NULL,NULL,4,NULL,NULL,'EndEvent_1el3hoq','EndEvent',_binary '\0',2),(17,4,'Start','2019-05-06 14:21:11.516350','2019-05-06 14:21:11.541966','2019-05-06 14:21:11.553032',NULL,NULL,5,NULL,NULL,'StartEvent_1846cge','StartEvent',_binary '\0',7),(18,4,'Task 0','2019-05-06 14:21:11.556307','2019-05-06 14:21:11.560005','2019-05-06 14:21:11.773366',NULL,NULL,5,NULL,NULL,'Task_0p1lhmm','UserTask',_binary '\0',7),(19,4,'Fork_AB','2019-05-06 14:21:11.774323','2019-05-06 14:21:11.778616','2019-05-06 14:21:11.781215',NULL,NULL,5,NULL,NULL,'ExclusiveGateway_07emx2k','ParallelGateway',_binary '\0',7),(20,4,'Task A','2019-05-06 14:21:11.786944','2019-05-06 14:21:11.788501','2019-05-06 14:21:11.804438',NULL,NULL,5,NULL,NULL,'Task_0gnw4s9','UserTask',_binary '\0',8),(21,4,'Task B','2019-05-06 14:21:11.791267','2019-05-06 14:21:11.792617','2019-05-06 14:21:11.818141',NULL,NULL,5,NULL,NULL,'Task_0dtq8u2','UserTask',_binary '\0',9),(22,4,'Join_AB','2019-05-06 14:21:11.805439','2019-05-06 14:21:11.870616','2019-05-06 14:21:11.873765',NULL,NULL,5,NULL,NULL,'ExclusiveGateway_082abmy','ParallelGateway',_binary '\0',8),(23,4,'Fork_B','2019-05-06 14:21:11.819872','2019-05-06 14:21:11.821534','2019-05-06 14:21:11.822952',NULL,NULL,5,NULL,NULL,'ExclusiveGateway_0svb4en','ParallelGateway',_binary '\0',9),(24,4,'Task B1','2019-05-06 14:21:11.826170','2019-05-06 14:21:11.827836','2019-05-06 14:21:11.838400',NULL,NULL,5,NULL,NULL,'Task_1mcwerv','UserTask',_binary '\0',10),(25,4,'Task B2','2019-05-06 14:21:11.830751','2019-05-06 14:21:11.832261','2019-05-06 14:21:11.844337',NULL,NULL,5,NULL,NULL,'Task_000icim','UserTask',_binary '\0',11),(26,4,'Join_B','2019-05-06 14:21:11.839681','2019-05-06 14:21:11.847338','2019-05-06 14:21:11.866654',NULL,NULL,5,NULL,NULL,'ExclusiveGateway_08wqw3l','ParallelGateway',_binary '\0',10),(27,4,'Join_B','2019-05-06 14:21:11.845607','2019-05-06 14:21:11.847338','2019-05-06 14:21:11.866654',NULL,NULL,5,NULL,NULL,'ExclusiveGateway_08wqw3l','ParallelGateway',_binary '\0',11),(28,4,'Join_AB','2019-05-06 14:21:11.868795','2019-05-06 14:21:11.870616','2019-05-06 14:21:11.873765',NULL,NULL,5,NULL,NULL,'ExclusiveGateway_082abmy','ParallelGateway',_binary '\0',7),(29,4,'Task C','2019-05-06 14:21:11.875607','2019-05-06 14:21:11.877308','2019-05-06 14:21:11.883078',NULL,NULL,5,NULL,NULL,'Task_0k5jzhg','UserTask',_binary '\0',7),(30,4,'End','2019-05-06 14:21:11.884684','2019-05-06 14:21:11.886552','2019-05-06 14:21:11.889292',NULL,NULL,5,NULL,NULL,'EndEvent_1el3hoq','EndEvent',_binary '\0',7),(43,4,'theStart','2019-05-06 17:03:08.649504','2019-05-06 17:03:08.677726','2019-05-06 17:03:08.687567',NULL,NULL,9,NULL,NULL,'theStart','StartEvent',_binary '\0',15),(44,4,'outerSubProcess','2019-05-06 17:03:08.690647','2019-05-06 17:03:08.695050','2019-05-06 17:03:08.902407',NULL,NULL,9,NULL,NULL,'outerSubProcess','SubProcess',_binary '\0',15),(45,4,'outerSubProcessStart','2019-05-06 17:03:08.699729','2019-05-06 17:03:08.701574','2019-05-06 17:03:08.702808',NULL,NULL,9,44,NULL,'outerSubProcessStart','StartEvent',_binary '\0',16),(46,4,'innerSubProcess','2019-05-06 17:03:08.703698','2019-05-06 17:03:08.705049','2019-05-06 17:03:08.884635',NULL,NULL,9,44,NULL,'innerSubProcess','SubProcess',_binary '\0',16),(47,4,'innerSubProcessStart','2019-05-06 17:03:08.707370','2019-05-06 17:03:08.708550','2019-05-06 17:03:08.709899',NULL,NULL,9,46,NULL,'innerSubProcessStart','StartEvent',_binary '\0',17),(48,4,'Task in subprocess','2019-05-06 17:03:08.710714','2019-05-06 17:03:08.712078','2019-05-06 17:03:08.878332',NULL,NULL,9,46,NULL,'innerSubProcessTask','UserTask',_binary '\0',17),(49,4,'innerSubProcessEnd','2019-05-06 17:03:08.879415','2019-05-06 17:03:08.880851','2019-05-06 17:03:08.883133',NULL,NULL,9,46,NULL,'innerSubProcessEnd','EndEvent',_binary '\0',17),(50,4,'outerSubProcessEnd','2019-05-06 17:03:08.898316','2019-05-06 17:03:08.899934','2019-05-06 17:03:08.901265',NULL,NULL,9,44,NULL,'outerSubProcessEnd','EndEvent',_binary '\0',16),(51,4,'Task after subprocesses','2019-05-06 17:03:08.903724','2019-05-06 17:03:08.905194','2019-05-06 17:03:08.924313',NULL,NULL,9,NULL,NULL,'afterSubProcessTask','UserTask',_binary '\0',15),(52,4,'theEnd','2019-05-06 17:03:08.925599','2019-05-06 17:03:08.927461','2019-05-06 17:03:08.928787',NULL,NULL,9,NULL,NULL,'theEnd','EndEvent',_binary '\0',15),(53,4,'theStart','2019-05-06 17:03:24.560405','2019-05-06 17:03:24.589350','2019-05-06 17:03:24.601862',NULL,NULL,10,NULL,NULL,'theStart','StartEvent',_binary '\0',18),(54,4,'miScriptTask','2019-05-06 17:03:24.604301','2019-05-06 17:03:24.608044','2019-05-06 17:03:24.874462',NULL,NULL,10,NULL,NULL,'miScriptTask','ScriptTask',_binary '',18),(55,4,'miScriptTask','2019-05-06 17:03:24.763666','2019-05-06 17:03:24.767814','2019-05-06 17:03:24.821333',NULL,NULL,10,54,NULL,'miScriptTask','ScriptTask',_binary '\0',19),(56,4,'miScriptTask','2019-05-06 17:03:24.825469','2019-05-06 17:03:24.827906','2019-05-06 17:03:24.829807',NULL,NULL,10,54,NULL,'miScriptTask','ScriptTask',_binary '\0',20),(57,4,'miScriptTask','2019-05-06 17:03:24.831847','2019-05-06 17:03:24.834315','2019-05-06 17:03:24.836252',NULL,NULL,10,54,NULL,'miScriptTask','ScriptTask',_binary '\0',21),(58,4,'miScriptTask','2019-05-06 17:03:24.838632','2019-05-06 17:03:24.843443','2019-05-06 17:03:24.845482',NULL,NULL,10,54,NULL,'miScriptTask','ScriptTask',_binary '\0',22),(59,4,'miScriptTask','2019-05-06 17:03:24.847561','2019-05-06 17:03:24.850226','2019-05-06 17:03:24.852363',NULL,NULL,10,54,NULL,'miScriptTask','ScriptTask',_binary '\0',23),(60,4,'waitState','2019-05-06 17:03:24.875866','2019-05-06 17:03:24.877848','2019-05-06 17:03:24.913681',NULL,NULL,10,NULL,NULL,'waitState','ReceiveTask',_binary '\0',18),(61,4,'theEnd','2019-05-06 17:03:24.915030','2019-05-06 17:03:24.916851','2019-05-06 17:03:24.919429',NULL,NULL,10,NULL,NULL,'theEnd','EndEvent',_binary '\0',18),(110,4,'Start','2019-05-06 17:04:02.499005','2019-05-06 17:04:02.530344','2019-05-06 17:04:02.540639',NULL,NULL,14,NULL,NULL,'startevent1','StartEvent',_binary '\0',45),(111,4,'theTask','2019-05-06 17:04:02.542884','2019-05-06 17:04:02.546687','2019-05-06 17:04:02.550410',NULL,NULL,14,NULL,NULL,'theTask','UserTask',_binary '',45),(112,4,'End','2019-05-06 17:04:02.551276','2019-05-06 17:04:02.552821','2019-05-06 17:04:02.555209',NULL,NULL,14,NULL,NULL,'endevent1','EndEvent',_binary '\0',45),(417,4,'theStart','2019-05-06 18:10:02.339523','2019-05-06 18:10:02.369974','2019-05-06 18:10:02.381847',NULL,NULL,26,NULL,NULL,'theStart','StartEvent',_binary '\0',183),(418,4,'miSubProcess','2019-05-06 18:10:02.384847','2019-05-06 18:10:02.389105','2019-05-06 18:10:27.109178',NULL,NULL,26,NULL,NULL,'miSubProcess','SubProcess',_binary '',183),(419,4,'miSubProcess','2019-05-06 18:10:02.576565','2019-05-06 18:10:02.583129','2019-05-06 18:10:02.792976',NULL,NULL,26,418,NULL,'miSubProcess','SubProcess',_binary '\0',184),(420,4,'subProcessStart','2019-05-06 18:10:02.587513','2019-05-06 18:10:02.589148','2019-05-06 18:10:02.590683',NULL,NULL,26,419,NULL,'subProcessStart','StartEvent',_binary '\0',185),(421,4,'subProcessFork','2019-05-06 18:10:02.592266','2019-05-06 18:10:02.595218','2019-05-06 18:10:02.599084',NULL,NULL,26,419,NULL,'subProcessFork','ParallelGateway',_binary '\0',185),(422,4,'task one','2019-05-06 18:10:02.603585','2019-05-06 18:10:02.605467','2019-05-06 18:10:02.740072',NULL,NULL,26,419,NULL,'subProcessTask1','UserTask',_binary '\0',186),(423,4,'task two','2019-05-06 18:10:02.663679','2019-05-06 18:10:02.665803','2019-05-06 18:10:02.759802',NULL,NULL,26,419,NULL,'subProcessTask2','UserTask',_binary '\0',187),(424,4,'subProcessJoin','2019-05-06 18:10:02.741849','2019-05-06 18:10:02.763481','2019-05-06 18:10:02.783040',NULL,NULL,26,419,NULL,'subProcessJoin','ParallelGateway',_binary '\0',186),(425,4,'subProcessJoin','2019-05-06 18:10:02.761437','2019-05-06 18:10:02.763481','2019-05-06 18:10:02.783040',NULL,NULL,26,419,NULL,'subProcessJoin','ParallelGateway',_binary '\0',187),(426,4,'subProcessEnd','2019-05-06 18:10:02.785045','2019-05-06 18:10:02.787136','2019-05-06 18:10:02.790083',NULL,NULL,26,419,NULL,'subProcessEnd','EndEvent',_binary '\0',185),(427,4,'miSubProcess','2019-05-06 18:10:02.795785','2019-05-06 18:10:02.798511','2019-05-06 18:10:22.382255',NULL,NULL,26,418,NULL,'miSubProcess','SubProcess',_binary '\0',184),(428,4,'subProcessStart','2019-05-06 18:10:02.802313','2019-05-06 18:10:02.804506','2019-05-06 18:10:02.807468',NULL,NULL,26,427,NULL,'subProcessStart','StartEvent',_binary '\0',188),(429,4,'subProcessFork','2019-05-06 18:10:02.809188','2019-05-06 18:10:02.813921','2019-05-06 18:10:02.815992',NULL,NULL,26,427,NULL,'subProcessFork','ParallelGateway',_binary '\0',188),(430,4,'task one','2019-05-06 18:10:02.819858','2019-05-06 18:10:02.821916','2019-05-06 18:10:22.342643',NULL,NULL,26,427,NULL,'subProcessTask1','UserTask',_binary '\0',189),(431,4,'task two','2019-05-06 18:10:02.826101','2019-05-06 18:10:02.828360','2019-05-06 18:10:22.351077',NULL,NULL,26,427,NULL,'subProcessTask2','UserTask',_binary '\0',190),(432,4,'subProcessJoin','2019-05-06 18:10:22.344454','2019-05-06 18:10:22.363285','2019-05-06 18:10:22.369232',NULL,NULL,26,427,NULL,'subProcessJoin','ParallelGateway',_binary '\0',189),(433,4,'subProcessJoin','2019-05-06 18:10:22.355202','2019-05-06 18:10:22.363285','2019-05-06 18:10:22.369232',NULL,NULL,26,427,NULL,'subProcessJoin','ParallelGateway',_binary '\0',190),(434,4,'subProcessEnd','2019-05-06 18:10:22.372992','2019-05-06 18:10:22.377495','2019-05-06 18:10:22.379909',NULL,NULL,26,427,NULL,'subProcessEnd','EndEvent',_binary '\0',188),(435,4,'miSubProcess','2019-05-06 18:10:22.384658','2019-05-06 18:10:22.389327','2019-05-06 18:10:26.989545',NULL,NULL,26,418,NULL,'miSubProcess','SubProcess',_binary '\0',184),(436,4,'subProcessStart','2019-05-06 18:10:22.396756','2019-05-06 18:10:22.399224','2019-05-06 18:10:22.401880',NULL,NULL,26,435,NULL,'subProcessStart','StartEvent',_binary '\0',191),(437,4,'subProcessFork','2019-05-06 18:10:22.404188','2019-05-06 18:10:22.408752','2019-05-06 18:10:22.411076',NULL,NULL,26,435,NULL,'subProcessFork','ParallelGateway',_binary '\0',191),(438,4,'task one','2019-05-06 18:10:22.415916','2019-05-06 18:10:22.418865','2019-05-06 18:10:26.952943',NULL,NULL,26,435,NULL,'subProcessTask1','UserTask',_binary '\0',192),(439,4,'task two','2019-05-06 18:10:22.429765','2019-05-06 18:10:22.432540','2019-05-06 18:10:26.964033',NULL,NULL,26,435,NULL,'subProcessTask2','UserTask',_binary '\0',193),(440,4,'subProcessJoin','2019-05-06 18:10:26.956979','2019-05-06 18:10:26.969672','2019-05-06 18:10:26.975895',NULL,NULL,26,435,NULL,'subProcessJoin','ParallelGateway',_binary '\0',192),(441,4,'subProcessJoin','2019-05-06 18:10:26.966101','2019-05-06 18:10:26.969672','2019-05-06 18:10:26.975895',NULL,NULL,26,435,NULL,'subProcessJoin','ParallelGateway',_binary '\0',193),(442,4,'subProcessEnd','2019-05-06 18:10:26.978345','2019-05-06 18:10:26.981008','2019-05-06 18:10:26.986124',NULL,NULL,26,435,NULL,'subProcessEnd','EndEvent',_binary '\0',191),(443,4,'miSubProcess','2019-05-06 18:10:26.992234','2019-05-06 18:10:26.995678','2019-05-06 18:10:27.095822',NULL,NULL,26,418,NULL,'miSubProcess','SubProcess',_binary '\0',184),(444,4,'subProcessStart','2019-05-06 18:10:27.000779','2019-05-06 18:10:27.005342','2019-05-06 18:10:27.008162',NULL,NULL,26,443,NULL,'subProcessStart','StartEvent',_binary '\0',194),(445,4,'subProcessFork','2019-05-06 18:10:27.010285','2019-05-06 18:10:27.012969','2019-05-06 18:10:27.015402',NULL,NULL,26,443,NULL,'subProcessFork','ParallelGateway',_binary '\0',194),(446,4,'task one','2019-05-06 18:10:27.022228','2019-05-06 18:10:27.026478','2019-05-06 18:10:27.049268',NULL,NULL,26,443,NULL,'subProcessTask1','UserTask',_binary '\0',195),(447,4,'task two','2019-05-06 18:10:27.032017','2019-05-06 18:10:27.037370','2019-05-06 18:10:27.063151',NULL,NULL,26,443,NULL,'subProcessTask2','UserTask',_binary '\0',196),(448,4,'subProcessJoin','2019-05-06 18:10:27.052492','2019-05-06 18:10:27.070599','2019-05-06 18:10:27.078753',NULL,NULL,26,443,NULL,'subProcessJoin','ParallelGateway',_binary '\0',195),(449,4,'subProcessJoin','2019-05-06 18:10:27.066061','2019-05-06 18:10:27.070599','2019-05-06 18:10:27.078753',NULL,NULL,26,443,NULL,'subProcessJoin','ParallelGateway',_binary '\0',196),(450,4,'subProcessEnd','2019-05-06 18:10:27.081744','2019-05-06 18:10:27.085450','2019-05-06 18:10:27.090555',NULL,NULL,26,443,NULL,'subProcessEnd','EndEvent',_binary '\0',194),(451,4,'theEnd','2019-05-06 18:10:27.111959','2019-05-06 18:10:27.116107','2019-05-06 18:10:27.120565',NULL,NULL,26,NULL,NULL,'theEnd','EndEvent',_binary '\0',183),(452,4,'theStart','2019-05-06 18:13:52.441229','2019-05-06 18:13:52.467853','2019-05-06 18:13:52.479131',NULL,NULL,27,NULL,NULL,'theStart','StartEvent',_binary '\0',197),(453,4,'miScriptTask','2019-05-06 18:13:52.480938','2019-05-06 18:13:52.484417','2019-05-06 18:13:52.765683',NULL,NULL,27,NULL,NULL,'miScriptTask','ScriptTask',_binary '',197),(454,4,'miScriptTask','2019-05-06 18:13:52.647386','2019-05-06 18:13:52.651597','2019-05-06 18:13:52.714329',NULL,NULL,27,453,NULL,'miScriptTask','ScriptTask',_binary '\0',198),(455,4,'miScriptTask','2019-05-06 18:13:52.716360','2019-05-06 18:13:52.721401','2019-05-06 18:13:52.723873',NULL,NULL,27,453,NULL,'miScriptTask','ScriptTask',_binary '\0',198),(456,4,'miScriptTask','2019-05-06 18:13:52.726438','2019-05-06 18:13:52.728992','2019-05-06 18:13:52.730790',NULL,NULL,27,453,NULL,'miScriptTask','ScriptTask',_binary '\0',198),(457,4,'miScriptTask','2019-05-06 18:13:52.732304','2019-05-06 18:13:52.734717','2019-05-06 18:13:52.736725',NULL,NULL,27,453,NULL,'miScriptTask','ScriptTask',_binary '\0',198),(458,4,'miScriptTask','2019-05-06 18:13:52.738954','2019-05-06 18:13:52.742407','2019-05-06 18:13:52.744435',NULL,NULL,27,453,NULL,'miScriptTask','ScriptTask',_binary '\0',198),(459,4,'waitState','2019-05-06 18:13:52.767083','2019-05-06 18:13:52.769245','2019-05-06 18:13:52.808329',NULL,NULL,27,NULL,NULL,'waitState','ReceiveTask',_binary '\0',197),(460,4,'theEnd','2019-05-06 18:13:52.810115','2019-05-06 18:13:52.812026','2019-05-06 18:13:52.814801',NULL,NULL,27,NULL,NULL,'theEnd','EndEvent',_binary '\0',197),(464,4,'start','2019-05-06 18:16:53.411778','2019-05-06 18:16:53.437314','2019-05-06 18:16:53.447715',NULL,NULL,29,NULL,NULL,'StartEvent_0busnrn','StartEvent',_binary '\0',200),(465,4,'Hello Word','2019-05-06 18:16:53.450678','2019-05-06 18:16:53.454978','2019-05-06 18:16:53.760926',NULL,NULL,29,NULL,NULL,'Task_105g1f1','UserTask',_binary '\0',200),(466,4,'end','2019-05-06 18:16:53.762099','2019-05-06 18:16:53.763855','2019-05-06 18:16:53.766948',NULL,NULL,29,NULL,NULL,'EndEvent_1ruiztz','EndEvent',_binary '\0',200);
/*!40000 ALTER TABLE `bpm_act_inst` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_byte_array`
--

LOCK TABLES `bpm_byte_array` WRITE;
/*!40000 ALTER TABLE `bpm_byte_array` DISABLE KEYS */;
INSERT INTO `bpm_byte_array` VALUES (5,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:di=\"http://www.omg.org/spec/DD/20100524/DI\" id=\"Definitions_0gduspr\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"3.3.0\">\r\n  <bpmn:process id=\"nestedForkJoin\" isExecutable=\"true\">\r\n    <bpmn:startEvent id=\"StartEvent_1846cge\" name=\"Start\">\r\n      <bpmn:outgoing>SequenceFlow_0l0j3ur</bpmn:outgoing>\r\n    </bpmn:startEvent>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0l0j3ur\" sourceRef=\"StartEvent_1846cge\" targetRef=\"Task_0p1lhmm\" />\r\n    <bpmn:userTask id=\"Task_0p1lhmm\" name=\"Task 0\">\r\n      <bpmn:incoming>SequenceFlow_0l0j3ur</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0oa4vei</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0oa4vei\" sourceRef=\"Task_0p1lhmm\" targetRef=\"ExclusiveGateway_07emx2k\" />\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_07emx2k\" name=\"Fork_AB\">\r\n      <bpmn:incoming>SequenceFlow_0oa4vei</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0p676hj</bpmn:outgoing>\r\n      <bpmn:outgoing>SequenceFlow_0o7jgnv</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0p676hj\" sourceRef=\"ExclusiveGateway_07emx2k\" targetRef=\"Task_0gnw4s9\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0o7jgnv\" sourceRef=\"ExclusiveGateway_07emx2k\" targetRef=\"Task_0dtq8u2\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1j0774w\" sourceRef=\"Task_0dtq8u2\" targetRef=\"ExclusiveGateway_0svb4en\" />\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_0svb4en\" name=\"Fork_B\">\r\n      <bpmn:incoming>SequenceFlow_1j0774w</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_1xnd8cu</bpmn:outgoing>\r\n      <bpmn:outgoing>SequenceFlow_1ssyj16</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1xnd8cu\" sourceRef=\"ExclusiveGateway_0svb4en\" targetRef=\"Task_1mcwerv\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1ssyj16\" sourceRef=\"ExclusiveGateway_0svb4en\" targetRef=\"Task_000icim\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_12qhwnv\" sourceRef=\"Task_1mcwerv\" targetRef=\"ExclusiveGateway_08wqw3l\" />\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_08wqw3l\" name=\"Join_B\">\r\n      <bpmn:incoming>SequenceFlow_12qhwnv</bpmn:incoming>\r\n      <bpmn:incoming>SequenceFlow_1vb4y88</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0aur7wx</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1vb4y88\" sourceRef=\"Task_000icim\" targetRef=\"ExclusiveGateway_08wqw3l\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0okiyaz\" sourceRef=\"Task_0gnw4s9\" targetRef=\"ExclusiveGateway_082abmy\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0aur7wx\" sourceRef=\"ExclusiveGateway_08wqw3l\" targetRef=\"ExclusiveGateway_082abmy\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0m3addx\" sourceRef=\"ExclusiveGateway_082abmy\" targetRef=\"Task_0k5jzhg\" />\r\n    <bpmn:endEvent id=\"EndEvent_1el3hoq\" name=\"End\">\r\n      <bpmn:incoming>SequenceFlow_18pr8ga</bpmn:incoming>\r\n    </bpmn:endEvent>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_18pr8ga\" sourceRef=\"Task_0k5jzhg\" targetRef=\"EndEvent_1el3hoq\" />\r\n    <bpmn:userTask id=\"Task_0k5jzhg\" name=\"Task C\">\r\n      <bpmn:incoming>SequenceFlow_0m3addx</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_18pr8ga</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:userTask id=\"Task_1mcwerv\" name=\"Task B1\">\r\n      <bpmn:incoming>SequenceFlow_1xnd8cu</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_12qhwnv</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:userTask id=\"Task_000icim\" name=\"Task B2\">\r\n      <bpmn:incoming>SequenceFlow_1ssyj16</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_1vb4y88</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_082abmy\" name=\"Join_AB\">\r\n      <bpmn:incoming>SequenceFlow_0okiyaz</bpmn:incoming>\r\n      <bpmn:incoming>SequenceFlow_0aur7wx</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0m3addx</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:userTask id=\"Task_0gnw4s9\" name=\"Task A\">\r\n      <bpmn:incoming>SequenceFlow_0p676hj</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0okiyaz</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:userTask id=\"Task_0dtq8u2\" name=\"Task B\">\r\n      <bpmn:incoming>SequenceFlow_0o7jgnv</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_1j0774w</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n  </bpmn:process>\r\n  <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\">\r\n    <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"nestedForkJoin\">\r\n      <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_1846cge\">\r\n        <dc:Bounds x=\"156\" y=\"190\" width=\"36\" height=\"36\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"162\" y=\"233\" width=\"25\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0l0j3ur_di\" bpmnElement=\"SequenceFlow_0l0j3ur\">\r\n        <di:waypoint x=\"192\" y=\"208\" />\r\n        <di:waypoint x=\"242\" y=\"208\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"UserTask_1m80waq_di\" bpmnElement=\"Task_0p1lhmm\">\r\n        <dc:Bounds x=\"242\" y=\"168\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0oa4vei_di\" bpmnElement=\"SequenceFlow_0oa4vei\">\r\n        <di:waypoint x=\"342\" y=\"208\" />\r\n        <di:waypoint x=\"392\" y=\"208\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_0xmuvz1_di\" bpmnElement=\"ExclusiveGateway_07emx2k\">\r\n        <dc:Bounds x=\"392\" y=\"183\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"452\" y=\"201\" width=\"44\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0p676hj_di\" bpmnElement=\"SequenceFlow_0p676hj\">\r\n        <di:waypoint x=\"417\" y=\"183\" />\r\n        <di:waypoint x=\"417\" y=\"130\" />\r\n        <di:waypoint x=\"513\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0o7jgnv_di\" bpmnElement=\"SequenceFlow_0o7jgnv\">\r\n        <di:waypoint x=\"417\" y=\"233\" />\r\n        <di:waypoint x=\"417\" y=\"294\" />\r\n        <di:waypoint x=\"513\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1j0774w_di\" bpmnElement=\"SequenceFlow_1j0774w\">\r\n        <di:waypoint x=\"613\" y=\"294\" />\r\n        <di:waypoint x=\"684\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_06s0cjn_di\" bpmnElement=\"ExclusiveGateway_0svb4en\">\r\n        <dc:Bounds x=\"684\" y=\"269\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"690\" y=\"245\" width=\"37\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1xnd8cu_di\" bpmnElement=\"SequenceFlow_1xnd8cu\">\r\n        <di:waypoint x=\"734\" y=\"294\" />\r\n        <di:waypoint x=\"805\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1ssyj16_di\" bpmnElement=\"SequenceFlow_1ssyj16\">\r\n        <di:waypoint x=\"709\" y=\"319\" />\r\n        <di:waypoint x=\"709\" y=\"404\" />\r\n        <di:waypoint x=\"805\" y=\"404\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_12qhwnv_di\" bpmnElement=\"SequenceFlow_12qhwnv\">\r\n        <di:waypoint x=\"905\" y=\"294\" />\r\n        <di:waypoint x=\"976\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_1ri4krl_di\" bpmnElement=\"ExclusiveGateway_08wqw3l\">\r\n        <dc:Bounds x=\"976\" y=\"269\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"1035.5\" y=\"287\" width=\"35\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1vb4y88_di\" bpmnElement=\"SequenceFlow_1vb4y88\">\r\n        <di:waypoint x=\"905\" y=\"404\" />\r\n        <di:waypoint x=\"1001\" y=\"404\" />\r\n        <di:waypoint x=\"1001\" y=\"319\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0okiyaz_di\" bpmnElement=\"SequenceFlow_0okiyaz\">\r\n        <di:waypoint x=\"613\" y=\"130\" />\r\n        <di:waypoint x=\"976\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0aur7wx_di\" bpmnElement=\"SequenceFlow_0aur7wx\">\r\n        <di:waypoint x=\"1001\" y=\"269\" />\r\n        <di:waypoint x=\"1001\" y=\"155\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0m3addx_di\" bpmnElement=\"SequenceFlow_0m3addx\">\r\n        <di:waypoint x=\"1026\" y=\"130\" />\r\n        <di:waypoint x=\"1076\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"EndEvent_1el3hoq_di\" bpmnElement=\"EndEvent_1el3hoq\">\r\n        <dc:Bounds x=\"1226\" y=\"112\" width=\"36\" height=\"36\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"1234\" y=\"155\" width=\"20\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_18pr8ga_di\" bpmnElement=\"SequenceFlow_18pr8ga\">\r\n        <di:waypoint x=\"1176\" y=\"130\" />\r\n        <di:waypoint x=\"1226\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"UserTask_17lzzn5_di\" bpmnElement=\"Task_0k5jzhg\">\r\n        <dc:Bounds x=\"1076\" y=\"90\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_15hh6pl_di\" bpmnElement=\"Task_1mcwerv\">\r\n        <dc:Bounds x=\"805\" y=\"254\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_1l6zky8_di\" bpmnElement=\"Task_000icim\">\r\n        <dc:Bounds x=\"805\" y=\"364\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_1xjgnc1_di\" bpmnElement=\"ExclusiveGateway_082abmy\">\r\n        <dc:Bounds x=\"976\" y=\"105\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"980\" y=\"81\" width=\"42\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_1hgng71_di\" bpmnElement=\"Task_0gnw4s9\">\r\n        <dc:Bounds x=\"513\" y=\"90\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_0mcn6l1_di\" bpmnElement=\"Task_0dtq8u2\">\r\n        <dc:Bounds x=\"513\" y=\"254\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n    </bpmndi:BPMNPlane>\r\n  </bpmndi:BPMNDiagram>\r\n</bpmn:definitions>\r\n'),(6,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:di=\"http://www.omg.org/spec/DD/20100524/DI\" id=\"Definitions_0gduspr\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"3.3.0\">\r\n  <bpmn:process id=\"nestedForkJoin\" isExecutable=\"true\">\r\n    <bpmn:startEvent id=\"StartEvent_1846cge\" name=\"Start\">\r\n      <bpmn:outgoing>SequenceFlow_0l0j3ur</bpmn:outgoing>\r\n    </bpmn:startEvent>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0l0j3ur\" sourceRef=\"StartEvent_1846cge\" targetRef=\"Task_0p1lhmm\" />\r\n    <bpmn:userTask id=\"Task_0p1lhmm\" name=\"Task 0\">\r\n      <bpmn:incoming>SequenceFlow_0l0j3ur</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0oa4vei</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0oa4vei\" sourceRef=\"Task_0p1lhmm\" targetRef=\"ExclusiveGateway_07emx2k\" />\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_07emx2k\" name=\"Fork_AB\">\r\n      <bpmn:incoming>SequenceFlow_0oa4vei</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0p676hj</bpmn:outgoing>\r\n      <bpmn:outgoing>SequenceFlow_0o7jgnv</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0p676hj\" sourceRef=\"ExclusiveGateway_07emx2k\" targetRef=\"Task_0gnw4s9\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0o7jgnv\" sourceRef=\"ExclusiveGateway_07emx2k\" targetRef=\"Task_0dtq8u2\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1j0774w\" sourceRef=\"Task_0dtq8u2\" targetRef=\"ExclusiveGateway_0svb4en\" />\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_0svb4en\" name=\"Fork_B\">\r\n      <bpmn:incoming>SequenceFlow_1j0774w</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_1xnd8cu</bpmn:outgoing>\r\n      <bpmn:outgoing>SequenceFlow_1ssyj16</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1xnd8cu\" sourceRef=\"ExclusiveGateway_0svb4en\" targetRef=\"Task_1mcwerv\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1ssyj16\" sourceRef=\"ExclusiveGateway_0svb4en\" targetRef=\"Task_000icim\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_12qhwnv\" sourceRef=\"Task_1mcwerv\" targetRef=\"ExclusiveGateway_08wqw3l\" />\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_08wqw3l\" name=\"Join_B\">\r\n      <bpmn:incoming>SequenceFlow_12qhwnv</bpmn:incoming>\r\n      <bpmn:incoming>SequenceFlow_1vb4y88</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0aur7wx</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_1vb4y88\" sourceRef=\"Task_000icim\" targetRef=\"ExclusiveGateway_08wqw3l\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0okiyaz\" sourceRef=\"Task_0gnw4s9\" targetRef=\"ExclusiveGateway_082abmy\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0aur7wx\" sourceRef=\"ExclusiveGateway_08wqw3l\" targetRef=\"ExclusiveGateway_082abmy\" />\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_0m3addx\" sourceRef=\"ExclusiveGateway_082abmy\" targetRef=\"Task_0k5jzhg\" />\r\n    <bpmn:endEvent id=\"EndEvent_1el3hoq\" name=\"End\">\r\n      <bpmn:incoming>SequenceFlow_18pr8ga</bpmn:incoming>\r\n    </bpmn:endEvent>\r\n    <bpmn:sequenceFlow id=\"SequenceFlow_18pr8ga\" sourceRef=\"Task_0k5jzhg\" targetRef=\"EndEvent_1el3hoq\" />\r\n    <bpmn:userTask id=\"Task_0k5jzhg\" name=\"Task C\">\r\n      <bpmn:incoming>SequenceFlow_0m3addx</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_18pr8ga</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:userTask id=\"Task_1mcwerv\" name=\"Task B1\">\r\n      <bpmn:incoming>SequenceFlow_1xnd8cu</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_12qhwnv</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:userTask id=\"Task_000icim\" name=\"Task B2\">\r\n      <bpmn:incoming>SequenceFlow_1ssyj16</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_1vb4y88</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:parallelGateway id=\"ExclusiveGateway_082abmy\" name=\"Join_AB\">\r\n      <bpmn:incoming>SequenceFlow_0okiyaz</bpmn:incoming>\r\n      <bpmn:incoming>SequenceFlow_0aur7wx</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0m3addx</bpmn:outgoing>\r\n    </bpmn:parallelGateway>\r\n    <bpmn:userTask id=\"Task_0gnw4s9\" name=\"Task A\">\r\n      <bpmn:incoming>SequenceFlow_0p676hj</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_0okiyaz</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n    <bpmn:userTask id=\"Task_0dtq8u2\" name=\"Task B\">\r\n      <bpmn:incoming>SequenceFlow_0o7jgnv</bpmn:incoming>\r\n      <bpmn:outgoing>SequenceFlow_1j0774w</bpmn:outgoing>\r\n    </bpmn:userTask>\r\n  </bpmn:process>\r\n  <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\">\r\n    <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"nestedForkJoin\">\r\n      <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_1846cge\">\r\n        <dc:Bounds x=\"156\" y=\"190\" width=\"36\" height=\"36\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"162\" y=\"233\" width=\"25\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0l0j3ur_di\" bpmnElement=\"SequenceFlow_0l0j3ur\">\r\n        <di:waypoint x=\"192\" y=\"208\" />\r\n        <di:waypoint x=\"242\" y=\"208\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"UserTask_1m80waq_di\" bpmnElement=\"Task_0p1lhmm\">\r\n        <dc:Bounds x=\"242\" y=\"168\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0oa4vei_di\" bpmnElement=\"SequenceFlow_0oa4vei\">\r\n        <di:waypoint x=\"342\" y=\"208\" />\r\n        <di:waypoint x=\"392\" y=\"208\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_0xmuvz1_di\" bpmnElement=\"ExclusiveGateway_07emx2k\">\r\n        <dc:Bounds x=\"392\" y=\"183\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"452\" y=\"201\" width=\"44\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0p676hj_di\" bpmnElement=\"SequenceFlow_0p676hj\">\r\n        <di:waypoint x=\"417\" y=\"183\" />\r\n        <di:waypoint x=\"417\" y=\"130\" />\r\n        <di:waypoint x=\"513\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0o7jgnv_di\" bpmnElement=\"SequenceFlow_0o7jgnv\">\r\n        <di:waypoint x=\"417\" y=\"233\" />\r\n        <di:waypoint x=\"417\" y=\"294\" />\r\n        <di:waypoint x=\"513\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1j0774w_di\" bpmnElement=\"SequenceFlow_1j0774w\">\r\n        <di:waypoint x=\"613\" y=\"294\" />\r\n        <di:waypoint x=\"684\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_06s0cjn_di\" bpmnElement=\"ExclusiveGateway_0svb4en\">\r\n        <dc:Bounds x=\"684\" y=\"269\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"690\" y=\"245\" width=\"37\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1xnd8cu_di\" bpmnElement=\"SequenceFlow_1xnd8cu\">\r\n        <di:waypoint x=\"734\" y=\"294\" />\r\n        <di:waypoint x=\"805\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1ssyj16_di\" bpmnElement=\"SequenceFlow_1ssyj16\">\r\n        <di:waypoint x=\"709\" y=\"319\" />\r\n        <di:waypoint x=\"709\" y=\"404\" />\r\n        <di:waypoint x=\"805\" y=\"404\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_12qhwnv_di\" bpmnElement=\"SequenceFlow_12qhwnv\">\r\n        <di:waypoint x=\"905\" y=\"294\" />\r\n        <di:waypoint x=\"976\" y=\"294\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_1ri4krl_di\" bpmnElement=\"ExclusiveGateway_08wqw3l\">\r\n        <dc:Bounds x=\"976\" y=\"269\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"1035.5\" y=\"287\" width=\"35\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_1vb4y88_di\" bpmnElement=\"SequenceFlow_1vb4y88\">\r\n        <di:waypoint x=\"905\" y=\"404\" />\r\n        <di:waypoint x=\"1001\" y=\"404\" />\r\n        <di:waypoint x=\"1001\" y=\"319\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0okiyaz_di\" bpmnElement=\"SequenceFlow_0okiyaz\">\r\n        <di:waypoint x=\"613\" y=\"130\" />\r\n        <di:waypoint x=\"976\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0aur7wx_di\" bpmnElement=\"SequenceFlow_0aur7wx\">\r\n        <di:waypoint x=\"1001\" y=\"269\" />\r\n        <di:waypoint x=\"1001\" y=\"155\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0m3addx_di\" bpmnElement=\"SequenceFlow_0m3addx\">\r\n        <di:waypoint x=\"1026\" y=\"130\" />\r\n        <di:waypoint x=\"1076\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"EndEvent_1el3hoq_di\" bpmnElement=\"EndEvent_1el3hoq\">\r\n        <dc:Bounds x=\"1226\" y=\"112\" width=\"36\" height=\"36\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"1234\" y=\"155\" width=\"20\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNEdge id=\"SequenceFlow_18pr8ga_di\" bpmnElement=\"SequenceFlow_18pr8ga\">\r\n        <di:waypoint x=\"1176\" y=\"130\" />\r\n        <di:waypoint x=\"1226\" y=\"130\" />\r\n      </bpmndi:BPMNEdge>\r\n      <bpmndi:BPMNShape id=\"UserTask_17lzzn5_di\" bpmnElement=\"Task_0k5jzhg\">\r\n        <dc:Bounds x=\"1076\" y=\"90\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_15hh6pl_di\" bpmnElement=\"Task_1mcwerv\">\r\n        <dc:Bounds x=\"805\" y=\"254\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_1l6zky8_di\" bpmnElement=\"Task_000icim\">\r\n        <dc:Bounds x=\"805\" y=\"364\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"ParallelGateway_1xjgnc1_di\" bpmnElement=\"ExclusiveGateway_082abmy\">\r\n        <dc:Bounds x=\"976\" y=\"105\" width=\"50\" height=\"50\" />\r\n        <bpmndi:BPMNLabel>\r\n          <dc:Bounds x=\"980\" y=\"81\" width=\"42\" height=\"14\" />\r\n        </bpmndi:BPMNLabel>\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_1hgng71_di\" bpmnElement=\"Task_0gnw4s9\">\r\n        <dc:Bounds x=\"513\" y=\"90\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n      <bpmndi:BPMNShape id=\"UserTask_0mcn6l1_di\" bpmnElement=\"Task_0dtq8u2\">\r\n        <dc:Bounds x=\"513\" y=\"254\" width=\"100\" height=\"80\" />\r\n      </bpmndi:BPMNShape>\r\n    </bpmndi:BPMNPlane>\r\n  </bpmndi:BPMNDiagram>\r\n</bpmn:definitions>\r\n'),(8,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions id=\"definitions\" \r\n  xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\"\r\n  xmlns:activiti=\"http://activiti.org/bpmn\"\r\n  targetNamespace=\"Examples\">\r\n  \r\n  <process id=\"nestedSimpleSubProcess\" isExecutable=\"true\"> \r\n  \r\n    <startEvent id=\"theStart\" />\r\n    <sequenceFlow id=\"flow1\" sourceRef=\"theStart\" targetRef=\"outerSubProcess\" />\r\n    \r\n    \r\n    <subProcess id=\"outerSubProcess\">\r\n    \r\n      <startEvent id=\"outerSubProcessStart\" />\r\n      <sequenceFlow id=\"flow2\" sourceRef=\"outerSubProcessStart\" targetRef=\"innerSubProcess\" />\r\n      \r\n      \r\n      <subProcess id=\"innerSubProcess\">\r\n      \r\n        <startEvent id=\"innerSubProcessStart\" />\r\n        <sequenceFlow id=\"flow3\" sourceRef=\"innerSubProcessStart\" targetRef=\"innerSubProcessTask\" />\r\n        \r\n        <userTask id=\"innerSubProcessTask\" name=\"Task in subprocess\" />\r\n        <sequenceFlow id=\"flow4\" sourceRef=\"innerSubProcessTask\" targetRef=\"innerSubProcessEnd\" />\r\n        \r\n        <endEvent id=\"innerSubProcessEnd\" />\r\n      \r\n      </subProcess>\r\n      \r\n      <sequenceFlow id=\"flow5\" sourceRef=\"innerSubProcess\" targetRef=\"outerSubProcessEnd\" />\r\n      <endEvent id=\"outerSubProcessEnd\" />\r\n    \r\n    </subProcess>\r\n    <sequenceFlow id=\"flow6\" sourceRef=\"outerSubProcess\" targetRef=\"afterSubProcessTask\" />\r\n\r\n\r\n    <userTask id=\"afterSubProcessTask\" name=\"Task after subprocesses\" />\r\n    <sequenceFlow id=\"flow7\" sourceRef=\"afterSubProcessTask\" targetRef=\"theEnd\" />\r\n    \r\n    <endEvent id=\"theEnd\" />\r\n    \r\n  </process>\r\n\r\n</definitions>'),(9,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions id=\"definition\" \r\n  xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\"\r\n  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n  xmlns:activiti=\"http://activiti.org/bpmn\"\r\n  targetNamespace=\"Examples\">\r\n  \r\n  <process id=\"miParallelScriptTask\" isExecutable=\"true\">\r\n  \r\n    <startEvent id=\"theStart\" />\r\n    <sequenceFlow id=\"flow1\" sourceRef=\"theStart\" targetRef=\"miScriptTask\" />\r\n    \r\n    <scriptTask id=\"miScriptTask\" scriptFormat=\"groovy\">\r\n      <multiInstanceLoopCharacteristics isSequential=\"false\">\r\n        <loopCardinality>${nrOfLoops}</loopCardinality>\r\n      </multiInstanceLoopCharacteristics>\r\n      <script>\r\n        <![CDATA[\r\n	        var processInstance = execution.ProcessInstance;\r\n	        var sum = processInstance.GetVariable(\"sum\")\r\n	        processInstance.SetVariable(\"sum\", sum + loopCounter)\r\n        ]]>\r\n      </script>\r\n    </scriptTask>\r\n    \r\n    <sequenceFlow id=\"flow3\" sourceRef=\"miScriptTask\" targetRef=\"waitState\" />\r\n    <receiveTask id=\"waitState\" />\r\n    \r\n     <sequenceFlow id=\"flow4\" sourceRef=\"waitState\" targetRef=\"theEnd\" />\r\n    <endEvent id=\"theEnd\" />\r\n    \r\n  </process>\r\n\r\n</definitions>'),(13,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" \r\n             xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" \r\n             xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"\r\n             xmlns:bpmtk=\"http://www.bpmtk.com/bpmn/extensions\"\r\n             xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" \r\n             xmlns:omgdc=\"http://www.omg.org/spec/DD/20100524/DC\" \r\n             xmlns:omgdi=\"http://www.omg.org/spec/DD/20100524/DI\" \r\n             typeLanguage=\"http://www.w3.org/2001/XMLSchema\" \r\n             expressionLanguage=\"http://www.w3.org/1999/XPath\" \r\n             targetNamespace=\"http://www.activiti.org/test\">\r\n  <process id=\"miSequentialUserTasksEmptyCollection\" name=\"My process\" isExecutable=\"true\">\r\n    <dataObject id=\"messages\" name=\"messages\" isCollection=\"true\" itemSubjectRef=\"xsd:string\">\r\n      <extensionElements>\r\n        <bpmtk:value>1</bpmtk:value>\r\n        <bpmtk:value>2</bpmtk:value>\r\n      </extensionElements>\r\n    </dataObject>\r\n    <startEvent id=\"startevent1\" name=\"Start\"></startEvent>\r\n    <userTask id=\"theTask\">\r\n    	<multiInstanceLoopCharacteristics isSequential=\"true\">\r\n        <loopDataInputRef>messages</loopDataInputRef>\r\n        <inputDataItem id=\"msg\"></inputDataItem>\r\n      </multiInstanceLoopCharacteristics>\r\n    </userTask>\r\n    <sequenceFlow id=\"flow1\" sourceRef=\"startevent1\" targetRef=\"theTask\"></sequenceFlow>\r\n    <endEvent id=\"endevent1\" name=\"End\"></endEvent>\r\n    <sequenceFlow id=\"flow2\" sourceRef=\"theTask\" targetRef=\"endevent1\"></sequenceFlow>\r\n  </process>\r\n</definitions>'),(14,_binary '\0\0\0\0ÿÿÿÿ\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(21,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions id=\"definition\"\r\n  xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\"\r\n  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n  xmlns:bpmtk=\"http://www.bpmtk.com/bpmn/extensions\"\r\n  targetNamespace=\"Examples\">\r\n\r\n  <process id=\"miSequentialSubprocess\" isExecutable=\"true\">\r\n\r\n    <startEvent id=\"theStart\" />\r\n    <sequenceFlow id=\"flow1\" sourceRef=\"theStart\" targetRef=\"miSubProcess\" />\r\n\r\n    <subProcess id=\"miSubProcess\">\r\n      <multiInstanceLoopCharacteristics isSequential=\"true\">\r\n        <loopCardinality>4</loopCardinality>\r\n      </multiInstanceLoopCharacteristics>\r\n\r\n      <startEvent id=\"subProcessStart\" />\r\n      <sequenceFlow id=\"subFlow1\" sourceRef=\"subProcessStart\" targetRef=\"subProcessFork\" />\r\n\r\n      <parallelGateway id=\"subProcessFork\" />\r\n      <sequenceFlow id=\"subFlow2\" sourceRef=\"subProcessFork\" targetRef=\"subProcessTask1\" />\r\n      <sequenceFlow id=\"subFlow3\" sourceRef=\"subProcessFork\" targetRef=\"subProcessTask2\" />\r\n\r\n      <userTask id=\"subProcessTask1\" name=\"task one\" />\r\n      <sequenceFlow id=\"subFlow4\" sourceRef=\"subProcessTask1\" targetRef=\"subProcessJoin\" />\r\n\r\n      <userTask id=\"subProcessTask2\" name=\"task two\" />\r\n      <sequenceFlow id=\"subFlow5\" sourceRef=\"subProcessTask2\" targetRef=\"subProcessJoin\" />\r\n\r\n      <parallelGateway id=\"subProcessJoin\" />\r\n\r\n      <sequenceFlow id=\"subFlow6\" sourceRef=\"subProcessJoin\" targetRef=\"subProcessEnd\" />\r\n\r\n      <endEvent id=\"subProcessEnd\" />\r\n\r\n    </subProcess>\r\n\r\n    <sequenceFlow id=\"flow3\" sourceRef=\"miSubProcess\" targetRef=\"theEnd\" />\r\n    <endEvent id=\"theEnd\" />\r\n\r\n  </process>\r\n\r\n</definitions>'),(26,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions id=\"definition\"\r\n  xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\"\r\n  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n  xmlns:bpmtk=\"http://www.bpmtk.com/bpmn/extensions\"\r\n  targetNamespace=\"Examples\">\r\n\r\n  <process id=\"miSequentialSubprocess\" isExecutable=\"true\">\r\n\r\n    <startEvent id=\"theStart\" />\r\n    <sequenceFlow id=\"flow1\" sourceRef=\"theStart\" targetRef=\"miSubProcess\" />\r\n\r\n    <subProcess id=\"miSubProcess\">\r\n      <multiInstanceLoopCharacteristics isSequential=\"true\">\r\n        <loopCardinality>4</loopCardinality>\r\n      </multiInstanceLoopCharacteristics>\r\n\r\n      <startEvent id=\"subProcessStart\" />\r\n      <sequenceFlow id=\"subFlow1\" sourceRef=\"subProcessStart\" targetRef=\"subProcessFork\" />\r\n\r\n      <parallelGateway id=\"subProcessFork\" />\r\n      <sequenceFlow id=\"subFlow2\" sourceRef=\"subProcessFork\" targetRef=\"subProcessTask1\" />\r\n      <sequenceFlow id=\"subFlow3\" sourceRef=\"subProcessFork\" targetRef=\"subProcessTask2\" />\r\n\r\n      <userTask id=\"subProcessTask1\" name=\"task one\" />\r\n      <sequenceFlow id=\"subFlow4\" sourceRef=\"subProcessTask1\" targetRef=\"subProcessJoin\" />\r\n\r\n      <userTask id=\"subProcessTask2\" name=\"task two\" />\r\n      <sequenceFlow id=\"subFlow5\" sourceRef=\"subProcessTask2\" targetRef=\"subProcessJoin\" />\r\n\r\n      <parallelGateway id=\"subProcessJoin\" />\r\n\r\n      <sequenceFlow id=\"subFlow6\" sourceRef=\"subProcessJoin\" targetRef=\"subProcessEnd\" />\r\n\r\n      <endEvent id=\"subProcessEnd\" />\r\n\r\n    </subProcess>\r\n\r\n    <sequenceFlow id=\"flow3\" sourceRef=\"miSubProcess\" targetRef=\"theEnd\" />\r\n    <endEvent id=\"theEnd\" />\r\n\r\n  </process>\r\n\r\n</definitions>'),(27,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions id=\"definition\" \r\n  xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\"\r\n  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n  xmlns:bpmtk=\"http://www.bpmtk.com/bpmn/extensions\"\r\n  targetNamespace=\"Examples\">\r\n  \r\n  <process id=\"miSequentialScriptTask\" isExecutable=\"true\">\r\n  \r\n    <startEvent id=\"theStart\" />\r\n    <sequenceFlow id=\"flow1\" sourceRef=\"theStart\" targetRef=\"miScriptTask\" />\r\n    \r\n    <scriptTask id=\"miScriptTask\">\r\n      <multiInstanceLoopCharacteristics isSequential=\"true\">\r\n        <loopCardinality>${nrOfLoops}</loopCardinality>\r\n      </multiInstanceLoopCharacteristics>\r\n      <script>\r\n        <![CDATA[\r\n	        var processInstance = execution.ProcessInstance;\r\n	        var sum = processInstance.GetVariable(\"sum\");\r\n	        processInstance.SetVariable(\"sum\", sum + loopCounter);\r\n        ]]>\r\n      </script>\r\n    </scriptTask>\r\n    \r\n    <sequenceFlow id=\"flow3\" sourceRef=\"miScriptTask\" targetRef=\"waitState\" />\r\n    <receiveTask id=\"waitState\" />\r\n    \r\n    <sequenceFlow id=\"flow4\" sourceRef=\"waitState\" targetRef=\"theEnd\" />\r\n    <endEvent id=\"theEnd\" />\r\n    \r\n  </process>\r\n\r\n</definitions>'),(29,_binary '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<bpmn:definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" \n                  xmlns:bpmn=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" \n                  xmlns:bpmtk=\"http://www.bpmtk.com/bpmn/extensions\"\n                  xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:dc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:di=\"http://www.omg.org/spec/DD/20100524/DI\" id=\"Definitions_0f2yaoj\" targetNamespace=\"http://bpmn.io/schema/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"3.3.0\">\n  <bpmn:process id=\"AssignTaskByVariableTestCase\" isExecutable=\"true\">\n    <bpmn:startEvent id=\"StartEvent_0busnrn\" name=\"start\">\n      <bpmn:outgoing>SequenceFlow_0bbuw2i</bpmn:outgoing>\n    </bpmn:startEvent>\n    <bpmn:userTask id=\"Task_105g1f1\" name=\"Hello Word\">\n      <bpmn:extensionElements>\n        <bpmtk:script on=\"start\">\n          <![CDATA[execution.SetVariable(\'user\', \'felix\')]]>\n        </bpmtk:script>\n        <bpmtk:attribute name=\"assignee\">${user}</bpmtk:attribute>\n        <bpmtk:attribute name=\"name\">${user + \'-\' + new Date().toLocaleString()}</bpmtk:attribute>\n        <bpmtk:attribute name=\"priority\">199</bpmtk:attribute>\n        <bpmtk:attribute name=\"duration\">D29H15</bpmtk:attribute>\n      </bpmn:extensionElements>\n      <bpmn:incoming>SequenceFlow_0bbuw2i</bpmn:incoming>\n      <bpmn:outgoing>SequenceFlow_03g0j1u</bpmn:outgoing>\n    </bpmn:userTask>\n    <bpmn:sequenceFlow id=\"SequenceFlow_0bbuw2i\" sourceRef=\"StartEvent_0busnrn\" targetRef=\"Task_105g1f1\" />\n    <bpmn:endEvent id=\"EndEvent_1ruiztz\" name=\"end\">\n      <bpmn:incoming>SequenceFlow_03g0j1u</bpmn:incoming>\n    </bpmn:endEvent>\n    <bpmn:sequenceFlow id=\"SequenceFlow_03g0j1u\" sourceRef=\"Task_105g1f1\" targetRef=\"EndEvent_1ruiztz\" />\n  </bpmn:process>\n  <bpmndi:BPMNDiagram id=\"BPMNDiagram_1\">\n    <bpmndi:BPMNPlane id=\"BPMNPlane_1\" bpmnElement=\"BpmnInlineScriptEventTestCase\">\n      <bpmndi:BPMNShape id=\"_BPMNShape_StartEvent_2\" bpmnElement=\"StartEvent_0busnrn\">\n        <dc:Bounds x=\"156\" y=\"103\" width=\"36\" height=\"36\" />\n        <bpmndi:BPMNLabel>\n          <dc:Bounds x=\"163\" y=\"146\" width=\"23\" height=\"14\" />\n        </bpmndi:BPMNLabel>\n      </bpmndi:BPMNShape>\n      <bpmndi:BPMNShape id=\"Task_105g1f1_di\" bpmnElement=\"Task_105g1f1\">\n        <dc:Bounds x=\"288\" y=\"81\" width=\"100\" height=\"80\" />\n      </bpmndi:BPMNShape>\n      <bpmndi:BPMNEdge id=\"SequenceFlow_0bbuw2i_di\" bpmnElement=\"SequenceFlow_0bbuw2i\">\n        <di:waypoint x=\"192\" y=\"121\" />\n        <di:waypoint x=\"288\" y=\"121\" />\n      </bpmndi:BPMNEdge>\n      <bpmndi:BPMNShape id=\"EndEvent_1ruiztz_di\" bpmnElement=\"EndEvent_1ruiztz\">\n        <dc:Bounds x=\"481\" y=\"103\" width=\"36\" height=\"36\" />\n        <bpmndi:BPMNLabel>\n          <dc:Bounds x=\"490\" y=\"146\" width=\"19\" height=\"14\" />\n        </bpmndi:BPMNLabel>\n      </bpmndi:BPMNShape>\n      <bpmndi:BPMNEdge id=\"SequenceFlow_03g0j1u_di\" bpmnElement=\"SequenceFlow_03g0j1u\">\n        <di:waypoint x=\"388\" y=\"121\" />\n        <di:waypoint x=\"481\" y=\"121\" />\n      </bpmndi:BPMNEdge>\n    </bpmndi:BPMNPlane>\n  </bpmndi:BPMNDiagram>\n</bpmn:definitions>\n');
/*!40000 ALTER TABLE `bpm_byte_array` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_comment`
--

LOCK TABLES `bpm_comment` WRITE;
/*!40000 ALTER TABLE `bpm_comment` DISABLE KEYS */;
/*!40000 ALTER TABLE `bpm_comment` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_deployment`
--

LOCK TABLES `bpm_deployment` WRITE;
/*!40000 ALTER TABLE `bpm_deployment` DISABLE KEYS */;
INSERT INTO `bpm_deployment` VALUES (5,'Demo deployment',NULL,'demo',5,'2019-05-06 12:49:03.299768',NULL,NULL,'A simple process demo for BPMTK.'),(6,'unit-tests',NULL,'tests',6,'2019-05-06 14:21:11.006857',NULL,NULL,NULL),(8,'unit-tests',NULL,'tests',8,'2019-05-06 17:03:08.027371',NULL,NULL,NULL),(9,'unit-tests',NULL,'tests',9,'2019-05-06 17:03:23.930607',NULL,NULL,NULL),(13,'unit-tests',NULL,'tests',13,'2019-05-06 17:04:01.839443',NULL,NULL,NULL),(20,'unit-tests',NULL,'tests',21,'2019-05-06 17:41:45.633443',NULL,NULL,NULL),(25,'unit-tests',NULL,'tests',26,'2019-05-06 18:10:01.657210',NULL,NULL,NULL),(26,'unit-tests',NULL,'tests',27,'2019-05-06 18:13:51.796869',NULL,NULL,NULL),(28,'unit-tests',NULL,'tests',29,'2019-05-06 18:16:52.793593',NULL,NULL,NULL);
/*!40000 ALTER TABLE `bpm_deployment` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_event_subscr`
--

LOCK TABLES `bpm_event_subscr` WRITE;
/*!40000 ALTER TABLE `bpm_event_subscr` DISABLE KEYS */;
/*!40000 ALTER TABLE `bpm_event_subscr` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_group`
--

LOCK TABLES `bpm_group` WRITE;
/*!40000 ALTER TABLE `bpm_group` DISABLE KEYS */;
INSERT INTO `bpm_group` VALUES ('tests','tests');
/*!40000 ALTER TABLE `bpm_group` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_identity_link`
--

LOCK TABLES `bpm_identity_link` WRITE;
/*!40000 ALTER TABLE `bpm_identity_link` DISABLE KEYS */;
/*!40000 ALTER TABLE `bpm_identity_link` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_package`
--

LOCK TABLES `bpm_package` WRITE;
/*!40000 ALTER TABLE `bpm_package` DISABLE KEYS */;
/*!40000 ALTER TABLE `bpm_package` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_proc_data`
--

LOCK TABLES `bpm_proc_data` WRITE;
/*!40000 ALTER TABLE `bpm_proc_data` DISABLE KEYS */;
INSERT INTO `bpm_proc_data` VALUES (10,4,'sum','int',NULL,NULL,NULL,10,NULL,NULL,NULL),(10,5,'nrOfLoops','int',NULL,NULL,NULL,5,NULL,NULL,NULL),(14,30,'messages','list',14,NULL,NULL,NULL,NULL,NULL,NULL),(27,79,'sum','int',NULL,NULL,NULL,10,NULL,NULL,NULL),(27,80,'nrOfLoops','int',NULL,NULL,NULL,5,NULL,NULL,NULL),(29,86,'user','string',NULL,'felix',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `bpm_proc_data` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_proc_def`
--

LOCK TABLES `bpm_proc_def` WRITE;
/*!40000 ALTER TABLE `bpm_proc_def` DISABLE KEYS */;
INSERT INTO `bpm_proc_def` VALUES (5,NULL,'demo',5,'nestedForkJoin','nestedForkJoin',1,'2019-05-06 12:49:03.299768','2019-05-06 12:49:03.299768',_binary '',NULL,NULL,NULL,1,NULL,NULL),(6,NULL,'tests',6,'nestedForkJoin','nestedForkJoin',2,'2019-05-06 14:21:11.006857','2019-05-06 14:21:11.006857',_binary '',NULL,NULL,NULL,1,NULL,NULL),(8,NULL,'tests',8,'nestedSimpleSubProcess','nestedSimpleSubProcess',1,'2019-05-06 17:03:08.027371','2019-05-06 17:03:08.027371',_binary '\0',NULL,NULL,NULL,1,NULL,NULL),(9,NULL,'tests',9,'miParallelScriptTask','miParallelScriptTask',1,'2019-05-06 17:03:23.930607','2019-05-06 17:03:23.930607',_binary '\0',NULL,NULL,NULL,1,NULL,NULL),(13,NULL,'tests',13,'My process','miSequentialUserTasksEmptyCollection',1,'2019-05-06 17:04:01.839443','2019-05-06 17:04:01.839443',_binary '\0',NULL,NULL,NULL,1,NULL,NULL),(20,NULL,'tests',20,'miSequentialSubprocess','miSequentialSubprocess',1,'2019-05-06 17:41:45.633443','2019-05-06 17:41:45.633443',_binary '\0',NULL,NULL,NULL,1,NULL,NULL),(25,NULL,'tests',25,'miSequentialSubprocess','miSequentialSubprocess',2,'2019-05-06 18:10:01.657210','2019-05-06 18:10:01.657210',_binary '\0',NULL,NULL,NULL,1,NULL,NULL),(26,NULL,'tests',26,'miSequentialScriptTask','miSequentialScriptTask',1,'2019-05-06 18:13:51.796869','2019-05-06 18:13:51.796869',_binary '\0',NULL,NULL,NULL,1,NULL,NULL),(28,NULL,'tests',28,'AssignTaskByVariableTestCase','AssignTaskByVariableTestCase',1,'2019-05-06 18:16:52.793593','2019-05-06 18:16:52.793593',_binary '\0',NULL,NULL,NULL,1,NULL,NULL);
/*!40000 ALTER TABLE `bpm_proc_def` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_proc_inst`
--

LOCK TABLES `bpm_proc_inst` WRITE;
/*!40000 ALTER TABLE `bpm_proc_inst` DISABLE KEYS */;
INSERT INTO `bpm_proc_inst` VALUES (4,4,'nestedForkJoin','2019-05-06 12:49:03.713028','2019-05-06 12:49:08.867775','2019-05-06 12:49:09.335979',NULL,NULL,NULL,NULL,'test',NULL,NULL,5,NULL),(5,4,'nestedForkJoin','2019-05-06 14:21:11.382223','2019-05-06 14:21:11.449113','2019-05-06 14:21:11.890969',NULL,NULL,NULL,NULL,'felix',NULL,NULL,6,NULL),(9,4,'nestedSimpleSubProcess','2019-05-06 17:03:08.515284','2019-05-06 17:03:08.606822','2019-05-06 17:03:08.929980',NULL,NULL,NULL,NULL,'felix',NULL,NULL,8,NULL),(10,4,'miParallelScriptTask','2019-05-06 17:03:24.400019','2019-05-06 17:03:24.517357','2019-05-06 17:03:24.921212',NULL,NULL,NULL,NULL,'felix',NULL,NULL,9,NULL),(14,4,'My process','2019-05-06 17:04:02.322066','2019-05-06 17:04:02.460047','2019-05-06 17:04:02.556571',NULL,NULL,NULL,NULL,'felix',NULL,NULL,13,NULL),(26,4,'miSequentialSubprocess','2019-05-06 18:10:02.208395','2019-05-06 18:10:02.291937','2019-05-06 18:10:27.123862',NULL,NULL,NULL,NULL,'felix',NULL,NULL,25,NULL),(27,4,'miSequentialScriptTask','2019-05-06 18:13:52.274053','2019-05-06 18:13:52.401714','2019-05-06 18:13:52.816416',NULL,NULL,NULL,NULL,'felix',NULL,NULL,26,NULL),(29,4,'AssignTaskByVariableTestCase','2019-05-06 18:16:53.293232','2019-05-06 18:16:53.372252','2019-05-06 18:16:53.768767',NULL,NULL,NULL,NULL,'felix',NULL,NULL,28,NULL);
/*!40000 ALTER TABLE `bpm_proc_inst` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_scheduled_job`
--

LOCK TABLES `bpm_scheduled_job` WRITE;
/*!40000 ALTER TABLE `bpm_scheduled_job` DISABLE KEYS */;
/*!40000 ALTER TABLE `bpm_scheduled_job` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_task`
--

LOCK TABLES `bpm_task` WRITE;
/*!40000 ALTER TABLE `bpm_task` DISABLE KEYS */;
INSERT INTO `bpm_task` VALUES (2,4,4,4,'2019-05-06 12:49:09.121717',NULL,'Task 0',0,'Task_0p1lhmm','2019-05-06 12:49:09.031354',NULL,NULL,NULL,'2019-05-06 12:49:09.031354',NULL,NULL),(3,4,6,4,'2019-05-06 12:49:09.197930',NULL,'Task A',0,'Task_0gnw4s9','2019-05-06 12:49:09.181704',NULL,NULL,NULL,'2019-05-06 12:49:09.181704',NULL,NULL),(4,4,7,4,'2019-05-06 12:49:09.208808',NULL,'Task B',0,'Task_0dtq8u2','2019-05-06 12:49:09.189190',NULL,NULL,NULL,'2019-05-06 12:49:09.189190',NULL,NULL),(5,4,10,4,'2019-05-06 12:49:09.241858',NULL,'Task B1',0,'Task_1mcwerv','2019-05-06 12:49:09.229794',NULL,NULL,NULL,'2019-05-06 12:49:09.229794',NULL,NULL),(6,4,11,4,'2019-05-06 12:49:09.250367',NULL,'Task B2',0,'Task_000icim','2019-05-06 12:49:09.238120',NULL,NULL,NULL,'2019-05-06 12:49:09.238120',NULL,NULL),(7,4,15,4,'2019-05-06 12:49:09.319401',NULL,'Task C',0,'Task_0k5jzhg','2019-05-06 12:49:09.315035',NULL,NULL,NULL,'2019-05-06 12:49:09.315035',NULL,NULL),(8,5,18,4,'2019-05-06 14:21:11.738400',NULL,'Task 0',0,'Task_0p1lhmm','2019-05-06 14:21:11.631646',NULL,NULL,NULL,'2019-05-06 14:21:11.631646',NULL,NULL),(9,5,20,4,'2019-05-06 14:21:11.802256',NULL,'Task A',0,'Task_0gnw4s9','2019-05-06 14:21:11.790121',NULL,NULL,NULL,'2019-05-06 14:21:11.790121',NULL,NULL),(10,5,21,4,'2019-05-06 14:21:11.813674',NULL,'Task B',0,'Task_0dtq8u2','2019-05-06 14:21:11.793976',NULL,NULL,NULL,'2019-05-06 14:21:11.793976',NULL,NULL),(11,5,24,4,'2019-05-06 14:21:11.836727',NULL,'Task B1',0,'Task_1mcwerv','2019-05-06 14:21:11.829411',NULL,NULL,NULL,'2019-05-06 14:21:11.829411',NULL,NULL),(12,5,25,4,'2019-05-06 14:21:11.841477',NULL,'Task B2',0,'Task_000icim','2019-05-06 14:21:11.834351',NULL,NULL,NULL,'2019-05-06 14:21:11.834351',NULL,NULL),(13,5,29,4,'2019-05-06 14:21:11.881067',NULL,'Task C',0,'Task_0k5jzhg','2019-05-06 14:21:11.878954',NULL,NULL,NULL,'2019-05-06 14:21:11.878954',NULL,NULL),(17,9,48,4,'2019-05-06 17:03:08.851406',NULL,'Task in subprocess',0,'innerSubProcessTask','2019-05-06 17:03:08.770795',NULL,NULL,NULL,'2019-05-06 17:03:08.770795',NULL,NULL),(18,9,51,4,'2019-05-06 17:03:08.922581',NULL,'Task after subprocesses',0,'afterSubProcessTask','2019-05-06 17:03:08.912073',NULL,NULL,NULL,'2019-05-06 17:03:08.912073',NULL,NULL),(99,26,422,4,'2019-05-06 18:10:02.708725',NULL,'task one',0,'subProcessTask1','2019-05-06 18:10:02.609309',NULL,NULL,NULL,'2019-05-06 18:10:02.609309',NULL,NULL),(100,26,423,4,'2019-05-06 18:10:02.749725',NULL,'task two',0,'subProcessTask2','2019-05-06 18:10:02.676173',NULL,NULL,NULL,'2019-05-06 18:10:02.676173',NULL,NULL),(101,26,430,4,'2019-05-06 18:10:22.338708',NULL,'task one',0,'subProcessTask1','2019-05-06 18:10:02.824014',NULL,NULL,NULL,'2019-05-06 18:10:02.824014',NULL,NULL),(102,26,431,4,'2019-05-06 18:10:22.346894',NULL,'task two',0,'subProcessTask2','2019-05-06 18:10:02.830760',NULL,NULL,NULL,'2019-05-06 18:10:02.830760',NULL,NULL),(103,26,438,4,'2019-05-06 18:10:26.947922',NULL,'task one',0,'subProcessTask1','2019-05-06 18:10:22.427155',NULL,NULL,NULL,'2019-05-06 18:10:22.427155',NULL,NULL),(104,26,439,4,'2019-05-06 18:10:26.959766',NULL,'task two',0,'subProcessTask2','2019-05-06 18:10:22.435652',NULL,NULL,NULL,'2019-05-06 18:10:22.435652',NULL,NULL),(105,26,446,4,'2019-05-06 18:10:27.046234',NULL,'task one',0,'subProcessTask1','2019-05-06 18:10:27.029339',NULL,NULL,NULL,'2019-05-06 18:10:27.029339',NULL,NULL),(106,26,447,4,'2019-05-06 18:10:27.057593',NULL,'task two',0,'subProcessTask2','2019-05-06 18:10:27.040450',NULL,NULL,NULL,'2019-05-06 18:10:27.040450',NULL,NULL),(108,29,465,4,'2019-05-06 18:16:53.731106',NULL,'felix-2019å¹´5æœˆ6æ—¥ 18:16:53',199,'Task_105g1f1','2019-05-06 18:16:53.621163',NULL,'felix',NULL,'2019-05-06 18:16:53.621163',NULL,NULL);
/*!40000 ALTER TABLE `bpm_task` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_token`
--

LOCK TABLES `bpm_token` WRITE;
/*!40000 ALTER TABLE `bpm_token` DISABLE KEYS */;
/*!40000 ALTER TABLE `bpm_token` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `bpm_user`
--

LOCK TABLES `bpm_user` WRITE;
/*!40000 ALTER TABLE `bpm_user` DISABLE KEYS */;
INSERT INTO `bpm_user` VALUES ('felix','felix'),('test','Test');
/*!40000 ALTER TABLE `bpm_user` ENABLE KEYS */;
UNLOCK TABLES;

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

--
-- Dumping data for table `bpm_user_group`
--

LOCK TABLES `bpm_user_group` WRITE;
/*!40000 ALTER TABLE `bpm_user_group` DISABLE KEYS */;
INSERT INTO `bpm_user_group` VALUES ('felix','tests');
/*!40000 ALTER TABLE `bpm_user_group` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-05-06 18:18:35
