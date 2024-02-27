/*
 Navicat Premium Data Transfer

 Source Server         : qmtdlt.top
 Source Server Type    : MySQL
 Source Server Version : 80200
 Source Host           : qmtdlt.top:3306
 Source Schema         : mydb

 Target Server Type    : MySQL
 Target Server Version : 80200
 File Encoding         : 65001

 Date: 27/02/2024 15:20:31
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for ip_change_log
-- ----------------------------
DROP TABLE IF EXISTS `ip_change_log`;
CREATE TABLE `ip_change_log`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `new_ip` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `time` datetime(3) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 57 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

SET FOREIGN_KEY_CHECKS = 1;
