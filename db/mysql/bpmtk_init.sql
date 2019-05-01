CREATE TABLE `bpm_act_data` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `type` varchar(255) NOT NULL,
  `text` text,
  `text2` text,
  `long_val` bigint(20) DEFAULT NULL,
  `double_val` double DEFAULT NULL,
  `byte_array_id` bigint(20) DEFAULT NULL,
  `act_inst_id` bigint(20) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `byte_array_id` (`byte_array_id`),
  KEY `byte_array_id_2` (`byte_array_id`),
  KEY `act_inst_id` (`act_inst_id`),
  CONSTRAINT `FK_6E0694C0` FOREIGN KEY (`byte_array_id`) REFERENCES `bpm_byte_array` (`id`),
  CONSTRAINT `FK_AD93DBF` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_act_inst` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `proc_inst_id` bigint(20) NOT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `sub_proc_inst_id` bigint(20) DEFAULT NULL,
  `name` varchar(100) NOT NULL,
  `state` int(11) NOT NULL,
  `activity_id` varchar(64) NOT NULL,
  `activity_type` varchar(32) NOT NULL,
  `created` datetime(6) NOT NULL,
  `is_mi_root` bit(1) NOT NULL DEFAULT b'0',
  `start_time` datetime(6) DEFAULT NULL,
  `last_state_time` datetime(6) NOT NULL,
  `concurrency_stamp` varchar(45) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `proc_inst_id` (`proc_inst_id`),
  KEY `parent_id` (`parent_id`),
  KEY `sub_proc_inst_id` (`sub_proc_inst_id`),
  CONSTRAINT `FK_A728FD4F` FOREIGN KEY (`parent_id`) REFERENCES `bpm_act_inst` (`id`),
  CONSTRAINT `FK_E2BAE114` FOREIGN KEY (`sub_proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_E6A04352` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=177 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_byte_array` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `value` longblob,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_comment` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `body` varchar(512) NOT NULL,
  `created` datetime NOT NULL,
  `user_id` int(11) NOT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `task_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_deployment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `package_id` int(11) DEFAULT NULL,
  `name` varchar(100) NOT NULL,
  `category` varchar(50) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `model_id` bigint(20) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `memo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `model_id` (`model_id`),
  KEY `package_id` (`package_id`),
  KEY `model_id_2` (`model_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `FK_4332136B` FOREIGN KEY (`user_id`) REFERENCES `bpm_user` (`id`),
  CONSTRAINT `FK_66D518AF` FOREIGN KEY (`package_id`) REFERENCES `bpm_package` (`id`),
  CONSTRAINT `FK_6DC27285` FOREIGN KEY (`model_id`) REFERENCES `bpm_byte_array` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_event_subscr` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  `event_type` varchar(50) NOT NULL,
  `event_name` varchar(50) NOT NULL,
  `activity_id` varchar(64) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `proc_def_id` (`proc_def_id`),
  KEY `proc_int_id` (`proc_inst_id`),
  KEY `token_id` (`token_id`),
  CONSTRAINT `FK_5B0C83F8` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`),
  CONSTRAINT `FK_8DB5D2C8` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`),
  CONSTRAINT `FK_FA5CDFF3` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_identity_link` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `type` varchar(50) DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `group_id` int(11) DEFAULT NULL,
  `task_id` bigint(20) DEFAULT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `act_inst_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `group_id` (`group_id`),
  KEY `task_id` (`task_id`),
  KEY `proc_def_id` (`proc_def_id`),
  KEY `proc_inst_id` (`proc_inst_id`),
  CONSTRAINT `FK_1E327124` FOREIGN KEY (`user_id`) REFERENCES `bpm_user` (`id`),
  CONSTRAINT `FK_2B48F45A` FOREIGN KEY (`task_id`) REFERENCES `bpm_task` (`id`),
  CONSTRAINT `FK_933AC53B` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`),
  CONSTRAINT `FK_D2440BFF` FOREIGN KEY (`group_id`) REFERENCES `bpm_group` (`id`),
  CONSTRAINT `FK_FE699ADB` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_package` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `name` varchar(100) NOT NULL,
  `category` varchar(50) DEFAULT NULL,
  `version` int(11) NOT NULL,
  `created` datetime(6) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `source_id` bigint(20) DEFAULT NULL,
  `owner_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `source_id` (`source_id`),
  KEY `owner_id` (`owner_id`),
  CONSTRAINT `FK_1ED113BD` FOREIGN KEY (`owner_id`) REFERENCES `bpm_user` (`id`),
  CONSTRAINT `FK_57851316` FOREIGN KEY (`source_id`) REFERENCES `bpm_byte_array` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_proc_data` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `type` varchar(255) NOT NULL,
  `text` text,
  `text2` text,
  `long_val` bigint(20) DEFAULT NULL,
  `double_val` double DEFAULT NULL,
  `byte_array_id` bigint(20) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  `task_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `byte_array_id` (`byte_array_id`),
  KEY `byte_array_id_2` (`byte_array_id`),
  KEY `proc_inst_id` (`proc_inst_id`),
  CONSTRAINT `FK_3481FDF4` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_A6950A84` FOREIGN KEY (`byte_array_id`) REFERENCES `bpm_byte_array` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_proc_def` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `deployment_id` int(11) NOT NULL,
  `key` varchar(64) NOT NULL,
  `name` varchar(100) NOT NULL,
  `state` int(11) NOT NULL,
  `category` varchar(50) DEFAULT NULL,
  `has_diagram` bit(1) NOT NULL DEFAULT b'0',
  `valid_from` datetime(6) DEFAULT NULL,
  `valid_to` datetime(6) DEFAULT NULL,
  `version` int(11) NOT NULL,
  `created` datetime(6) NOT NULL,
  `version_tag` varchar(255) DEFAULT NULL,
  `concurrency_stamp` varchar(50) DEFAULT NULL,
  `modified` datetime NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `deployment_id` (`deployment_id`),
  CONSTRAINT `FK_DDDE49C5` FOREIGN KEY (`deployment_id`) REFERENCES `bpm_deployment` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_proc_inst` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `proc_def_id` int(11) NOT NULL,
  `key` varchar(32) DEFAULT NULL,
  `name` varchar(100) NOT NULL,
  `state` int(11) NOT NULL,
  `created` datetime(6) NOT NULL,
  `start_time` datetime(6) DEFAULT NULL,
  `last_state_time` datetime(6) NOT NULL,
  `end_reason` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `caller_id` bigint(20) DEFAULT NULL,
  `super_id` bigint(20) DEFAULT NULL,
  `initiator_id` int(11) DEFAULT NULL,
  `concurrency_stamp` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `proc_def_id` (`proc_def_id`),
  KEY `call_act_id` (`caller_id`),
  KEY `super_id` (`super_id`),
  KEY `initiator_id` (`initiator_id`),
  CONSTRAINT `FK_43E81E56` FOREIGN KEY (`caller_id`) REFERENCES `bpm_act_inst` (`id`),
  CONSTRAINT `FK_5CC41CB3` FOREIGN KEY (`super_id`) REFERENCES `bpm_token` (`id`),
  CONSTRAINT `FK_B43276ED` FOREIGN KEY (`initiator_id`) REFERENCES `bpm_user` (`id`),
  CONSTRAINT `FK_F70DC20F` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_scheduled_job` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `tenant_id` varchar(32) DEFAULT NULL,
  `proc_def_id` int(11) DEFAULT NULL,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  `type` varchar(50) NOT NULL,
  `key` varchar(50) NOT NULL,
  `handler` varchar(100) NOT NULL,
  `retries` int(11) NOT NULL,
  `activity_id` varchar(64) DEFAULT NULL,
  `message` varchar(255) DEFAULT NULL,
  `stack_trace` varchar(255) DEFAULT NULL,
  `due_date` datetime(6) DEFAULT NULL,
  `end_date` datetime(6) DEFAULT NULL,
  `created` datetime(6) NOT NULL,
  `options` text,
  PRIMARY KEY (`id`),
  KEY `proc_def_id` (`proc_def_id`),
  KEY `proc_int_id` (`proc_inst_id`),
  KEY `token_id` (`token_id`),
  CONSTRAINT `FK_2AAA9B15` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`),
  CONSTRAINT `FK_4584D931` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_510E6DD5` FOREIGN KEY (`proc_def_id`) REFERENCES `bpm_proc_def` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `proc_inst_id` bigint(20) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `state` int(11) NOT NULL,
  `last_state_time` datetime(6) NOT NULL,
  `priority` smallint(6) NOT NULL,
  `due_date` datetime(6) DEFAULT NULL,
  `activity_id` varchar(64) DEFAULT NULL,
  `act_inst_id` bigint(20) DEFAULT NULL,
  `token_id` bigint(20) DEFAULT NULL,
  `assignee_id` int(11) DEFAULT NULL,
  `claimed_time` datetime(6) DEFAULT NULL,
  `concurrency_stamp` varchar(45) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `modified` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `proc_inst_id` (`proc_inst_id`),
  KEY `act_inst_id` (`act_inst_id`),
  KEY `token_id` (`token_id`),
  KEY `assignee_id` (`assignee_id`),
  CONSTRAINT `FK_181583BD` FOREIGN KEY (`assignee_id`) REFERENCES `bpm_user` (`id`),
  CONSTRAINT `FK_9FF81997` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`),
  CONSTRAINT `FK_A7D683CA` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_DB84D7B9` FOREIGN KEY (`token_id`) REFERENCES `bpm_token` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_token` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `proc_inst_id` bigint(20) NOT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `activity_id` varchar(64) DEFAULT NULL,
  `transition_id` varchar(64) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL,
  `is_suspended` tinyint(1) NOT NULL,
  `is_scope` bit(1) NOT NULL DEFAULT b'0',
  `is_mi_root` tinyint(1) NOT NULL,
  `act_inst_id` bigint(20) DEFAULT NULL,
  `sub_proc_inst_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `act_inst_id` (`act_inst_id`),
  KEY `proc_inst_id` (`proc_inst_id`),
  KEY `parent_id` (`parent_id`),
  CONSTRAINT `FK_4BE39448` FOREIGN KEY (`parent_id`) REFERENCES `bpm_token` (`id`),
  CONSTRAINT `FK_C1F837F1` FOREIGN KEY (`proc_inst_id`) REFERENCES `bpm_proc_inst` (`id`),
  CONSTRAINT `FK_C3B07BE7` FOREIGN KEY (`act_inst_id`) REFERENCES `bpm_act_inst` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `user_name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE `bpm_user_group` (
  `user_id` int(11) NOT NULL,
  `group_id` int(11) NOT NULL,
  PRIMARY KEY (`user_id`,`group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
