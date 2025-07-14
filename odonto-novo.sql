/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19  Distrib 10.6.22-MariaDB, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: odonto
-- ------------------------------------------------------
-- Server version	10.6.22-MariaDB-0ubuntu0.22.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `agendamento`
--

DROP TABLE IF EXISTS `agendamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `agendamento` (
  `id_agenda` int(11) NOT NULL AUTO_INCREMENT,
  `dt_agenda` date NOT NULL,
  `hr_agenda` int(11) NOT NULL,
  `id_dentista` int(11) NOT NULL,
  `id_crianca` int(11) NOT NULL,
  PRIMARY KEY (`id_agenda`),
  KEY `id_dentista` (`id_dentista`),
  KEY `id_crianca` (`id_crianca`),
  CONSTRAINT `agendamento_ibfk_1` FOREIGN KEY (`id_dentista`) REFERENCES `dentista` (`id_dentista`),
  CONSTRAINT `agendamento_ibfk_2` FOREIGN KEY (`id_crianca`) REFERENCES `crianca` (`id_crianca`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agendamento`
--

LOCK TABLES `agendamento` WRITE;
/*!40000 ALTER TABLE `agendamento` DISABLE KEYS */;
/*!40000 ALTER TABLE `agendamento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agendamento_instituicao`
--

DROP TABLE IF EXISTS `agendamento_instituicao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `agendamento_instituicao` (
  `id_instituicao` int(11) NOT NULL,
  `id_agenda` int(11) NOT NULL,
  PRIMARY KEY (`id_instituicao`,`id_agenda`),
  KEY `id_agenda` (`id_agenda`),
  CONSTRAINT `agendamento_instituicao_ibfk_1` FOREIGN KEY (`id_instituicao`) REFERENCES `instituicao` (`id_instituicao`),
  CONSTRAINT `agendamento_instituicao_ibfk_2` FOREIGN KEY (`id_agenda`) REFERENCES `agendamento` (`id_agenda`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agendamento_instituicao`
--

LOCK TABLES `agendamento_instituicao` WRITE;
/*!40000 ALTER TABLE `agendamento_instituicao` DISABLE KEYS */;
/*!40000 ALTER TABLE `agendamento_instituicao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agendamento_servico`
--

DROP TABLE IF EXISTS `agendamento_servico`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `agendamento_servico` (
  `id_servico` int(11) NOT NULL,
  `id_agenda` int(11) NOT NULL,
  PRIMARY KEY (`id_servico`,`id_agenda`),
  KEY `id_agenda` (`id_agenda`),
  CONSTRAINT `agendamento_servico_ibfk_1` FOREIGN KEY (`id_servico`) REFERENCES `servico` (`id_servico`),
  CONSTRAINT `agendamento_servico_ibfk_2` FOREIGN KEY (`id_agenda`) REFERENCES `agendamento` (`id_agenda`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agendamento_servico`
--

LOCK TABLES `agendamento_servico` WRITE;
/*!40000 ALTER TABLE `agendamento_servico` DISABLE KEYS */;
/*!40000 ALTER TABLE `agendamento_servico` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `atendimento`
--

DROP TABLE IF EXISTS `atendimento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `atendimento` (
  `id_atendimento` int(11) NOT NULL AUTO_INCREMENT,
  `observacao` varchar(100) NOT NULL,
  `dt_atendimento` date NOT NULL,
  `duracao_atendimento` int(11) NOT NULL,
  `hr_atendimento` int(11) NOT NULL,
  `id_crianca` int(11) NOT NULL,
  `id_dentista` int(11) NOT NULL,
  `id_agenda` int(11) NOT NULL,
  `id_odontograma` int(11) NOT NULL,
  PRIMARY KEY (`id_atendimento`),
  UNIQUE KEY `id_odontograma` (`id_odontograma`),
  KEY `id_crianca` (`id_crianca`),
  KEY `id_dentista` (`id_dentista`),
  KEY `id_agenda` (`id_agenda`),
  CONSTRAINT `atendimento_ibfk_1` FOREIGN KEY (`id_crianca`) REFERENCES `crianca` (`id_crianca`),
  CONSTRAINT `atendimento_ibfk_2` FOREIGN KEY (`id_dentista`) REFERENCES `dentista` (`id_dentista`),
  CONSTRAINT `atendimento_ibfk_3` FOREIGN KEY (`id_agenda`) REFERENCES `agendamento` (`id_agenda`),
  CONSTRAINT `atendimento_ibfk_4` FOREIGN KEY (`id_odontograma`) REFERENCES `odontograma` (`id_odontograma`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `atendimento`
--

LOCK TABLES `atendimento` WRITE;
/*!40000 ALTER TABLE `atendimento` DISABLE KEYS */;
/*!40000 ALTER TABLE `atendimento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `atendimento_servico`
--

DROP TABLE IF EXISTS `atendimento_servico`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `atendimento_servico` (
  `id_servico` int(11) NOT NULL,
  `id_atendimento` int(11) NOT NULL,
  PRIMARY KEY (`id_servico`,`id_atendimento`),
  KEY `id_atendimento` (`id_atendimento`),
  CONSTRAINT `atendimento_servico_ibfk_1` FOREIGN KEY (`id_servico`) REFERENCES `servico` (`id_servico`),
  CONSTRAINT `atendimento_servico_ibfk_2` FOREIGN KEY (`id_atendimento`) REFERENCES `atendimento` (`id_atendimento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `atendimento_servico`
--

LOCK TABLES `atendimento_servico` WRITE;
/*!40000 ALTER TABLE `atendimento_servico` DISABLE KEYS */;
/*!40000 ALTER TABLE `atendimento_servico` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `crianca`
--

DROP TABLE IF EXISTS `crianca`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `crianca` (
  `id_crianca` int(11) NOT NULL AUTO_INCREMENT,
  `nome_crianca` varchar(50) NOT NULL,
  `cpf_crianca` varchar(11) NOT NULL,
  `dt_nasc_crianca` date NOT NULL,
  `parentesco` varchar(20) NOT NULL,
  `id_resp` int(11) NOT NULL,
  PRIMARY KEY (`id_crianca`),
  UNIQUE KEY `cpf_crianca` (`cpf_crianca`),
  KEY `id_resp` (`id_resp`),
  CONSTRAINT `crianca_ibfk_1` FOREIGN KEY (`id_resp`) REFERENCES `responsavel` (`id_resp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `crianca`
--

LOCK TABLES `crianca` WRITE;
/*!40000 ALTER TABLE `crianca` DISABLE KEYS */;
/*!40000 ALTER TABLE `crianca` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dentista`
--

DROP TABLE IF EXISTS `dentista`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `dentista` (
  `id_dentista` int(11) NOT NULL AUTO_INCREMENT,
  `nome_dent` varchar(50) NOT NULL,
  `cpf_dent` varchar(11) NOT NULL,
  `cro` varchar(10) NOT NULL,
  `endereco_dent` varchar(60) NOT NULL,
  `email_dent` varchar(50) NOT NULL,
  `tel_dent` varchar(15) NOT NULL,
  PRIMARY KEY (`id_dentista`),
  UNIQUE KEY `cpf_dent` (`cpf_dent`),
  UNIQUE KEY `cro` (`cro`),
  UNIQUE KEY `email_dent` (`email_dent`),
  UNIQUE KEY `tel_dent` (`tel_dent`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dentista`
--

LOCK TABLES `dentista` WRITE;
/*!40000 ALTER TABLE `dentista` DISABLE KEYS */;
/*!40000 ALTER TABLE `dentista` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `escala_trabalho`
--

DROP TABLE IF EXISTS `escala_trabalho`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `escala_trabalho` (
  `id_escala` int(11) NOT NULL AUTO_INCREMENT,
  `dt_disponivel` date NOT NULL,
  `hr_inicio` int(11) NOT NULL,
  `hr_fim` int(11) NOT NULL,
  `id_dentista` int(11) NOT NULL,
  PRIMARY KEY (`id_escala`),
  KEY `id_dentista` (`id_dentista`),
  CONSTRAINT `escala_trabalho_ibfk_1` FOREIGN KEY (`id_dentista`) REFERENCES `dentista` (`id_dentista`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `escala_trabalho`
--

LOCK TABLES `escala_trabalho` WRITE;
/*!40000 ALTER TABLE `escala_trabalho` DISABLE KEYS */;
/*!40000 ALTER TABLE `escala_trabalho` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instituicao`
--

DROP TABLE IF EXISTS `instituicao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `instituicao` (
  `id_instituicao` int(11) NOT NULL AUTO_INCREMENT,
  `nome_instituicao` varchar(50) NOT NULL,
  `cnpj_instituicao` varchar(14) NOT NULL,
  `endereco_instituicao` varchar(60) NOT NULL,
  `email_instituicao` varchar(50) NOT NULL,
  `tel_instituicao` varchar(15) NOT NULL,
  PRIMARY KEY (`id_instituicao`),
  UNIQUE KEY `cnpj_instituicao` (`cnpj_instituicao`),
  UNIQUE KEY `email_instituicao` (`email_instituicao`),
  UNIQUE KEY `tel_instituicao` (`tel_instituicao`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instituicao`
--

LOCK TABLES `instituicao` WRITE;
/*!40000 ALTER TABLE `instituicao` DISABLE KEYS */;
/*!40000 ALTER TABLE `instituicao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `odontograma`
--

DROP TABLE IF EXISTS `odontograma`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `odontograma` (
  `id_odontograma` int(11) NOT NULL AUTO_INCREMENT,
  `quadrante` int(11) NOT NULL,
  `numero_dente` int(11) NOT NULL,
  `face` varchar(20) NOT NULL,
  PRIMARY KEY (`id_odontograma`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `odontograma`
--

LOCK TABLES `odontograma` WRITE;
/*!40000 ALTER TABLE `odontograma` DISABLE KEYS */;
/*!40000 ALTER TABLE `odontograma` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `responsavel`
--

DROP TABLE IF EXISTS `responsavel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `responsavel` (
  `id_resp` int(11) NOT NULL AUTO_INCREMENT,
  `nome_resp` varchar(50) NOT NULL,
  `cpf_resp` varchar(15) NOT NULL,
  `tel_resp` varchar(15) NOT NULL,
  `email_resp` varchar(50) NOT NULL,
  `endereco_resp` varchar(60) NOT NULL,
  `senha_resp` varchar(255) NOT NULL DEFAULT '',
  `data_cadastro` datetime NOT NULL DEFAULT current_timestamp(),
  `ativo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_resp`),
  UNIQUE KEY `cpf_resp` (`cpf_resp`),
  UNIQUE KEY `tel_resp` (`tel_resp`),
  UNIQUE KEY `email_resp` (`email_resp`),
  UNIQUE KEY `unique_email_resp` (`email_resp`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `responsavel`
--

LOCK TABLES `responsavel` WRITE;
/*!40000 ALTER TABLE `responsavel` DISABLE KEYS */;
INSERT INTO `responsavel` VALUES (1,'Juca Chicó','2665578412','11945784511','ze@buscape.com','Rua Nova Atibaia','','2025-07-11 13:38:46',1),(3,'Alonso','583.321.868-38','(11) 98765-4321','flango@gmail.com','Rua das Jurubebas, 37','','2025-07-11 13:38:46',1),(4,'Administrador','00000000000','(00) 00000-0000','admin@piodonto.com','Sistema','rFAnEbe7kty5F5OXSm9J+S8t1jnHcaloJ+P2kftbBzg=','2025-07-11 13:43:46',1),(6,'Julio Gafeia','146.388.751-57','(11) 95478-4542','julio@dez.com','Rua Tulio','','2025-07-11 14:09:23',1),(7,'Joao Lucas Alves Florencio','88140333310','11950704653','zina@zina.com','Rua do Queiroz, 25','rFAnEbe7kty5F5OXSm9J+S8t1jnHcaloJ+P2kftbBzg=','2025-07-11 15:04:13',1),(8,'Alice Alves Florencio','55333938890','11970234656','alice@gmail.com','Rua do queijo Branco,788','F82wx3gKCKJlTBNiXSdLEKber4WuUBnRlAnY03xtOlM=','2025-07-11 16:13:35',1),(9,'Guilherme Nunes de Oliveira','52644416850','11957221791','guilherme.ibnunes@gmail.com','Rua da Esperança, 91, Guarulhos 07157810','GYzXRRPugU/QyErKY2wp+nDXCGgY2z8Cr2XM8LiKKmI=','2025-07-11 17:12:23',1),(10,'Douglas Sirqueira ','25796967843','11965691720','douglaspaiva1@gmail.com','Rua Ernesto de Castro Neves 521','ANPZUKdgtVcKXep457B6k4oYT//Vr2C98jSfUtdO14I=','2025-07-11 22:25:59',1);
/*!40000 ALTER TABLE `responsavel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `servico`
--

DROP TABLE IF EXISTS `servico`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `servico` (
  `id_servico` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(50) NOT NULL,
  `duracao_servico` int(11) NOT NULL,
  PRIMARY KEY (`id_servico`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servico`
--

LOCK TABLES `servico` WRITE;
/*!40000 ALTER TABLE `servico` DISABLE KEYS */;
/*!40000 ALTER TABLE `servico` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(100) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'joaopaf','123456','admin');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-13 13:02:31
