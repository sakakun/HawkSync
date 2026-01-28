BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "tb_chatAutoMessages" (
	"autoMessageId"	INTEGER,
	"autoMessageText"	TEXT NOT NULL,
	"autoMessageTrigger"	INTEGER NOT NULL DEFAULT 0,
	PRIMARY KEY("autoMessageId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "tb_chatLogs" (
	"id"	INTEGER,
	"messageTimeStamp"	TEXT NOT NULL,
	"playerName"	TEXT NOT NULL,
	"messageType"	INTEGER NOT NULL,
	"messageType2"	INTEGER NOT NULL,
	"messageText"	TEXT NOT NULL,
	PRIMARY KEY("id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "tb_chatSlapMessages" (
	"slapMessageId"	INTEGER,
	"slapMessageText"	TEXT NOT NULL,
	PRIMARY KEY("slapMessageId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "tb_defaultMaps" (
	"mapID"	INTEGER UNIQUE,
	"mapName"	TEXT,
	"mapFile"	TEXT,
	"modType"	INTEGER,
	"mapType"	INTEGER,
	PRIMARY KEY("mapID")
);
CREATE TABLE IF NOT EXISTS "tb_mapPlaylists" (
	"playlistID"	INTEGER NOT NULL,
	"mapID"	INTEGER NOT NULL,
	"mapName"	TEXT,
	"mapFile"	TEXT,
	"modType"	INTEGER,
	"mapType"	INTEGER,
	PRIMARY KEY("playlistID","mapID")
);
CREATE TABLE IF NOT EXISTS "tb_playerIPRecords" (
	"RecordID"	INTEGER,
	"MatchID"	INTEGER NOT NULL DEFAULT 0,
	"PlayerIP"	TEXT NOT NULL,
	"SubnetMask"	INTEGER NOT NULL,
	"Date"	TEXT NOT NULL,
	"ExpireDate"	TEXT,
	"AssociatedName"	INTEGER,
	"RecordType"	INTEGER NOT NULL,
	"RecordCategory"	TEXT NOT NULL,
	"Notes"	TEXT DEFAULT '',
	PRIMARY KEY("RecordID" AUTOINCREMENT),
	FOREIGN KEY("AssociatedName") REFERENCES "tb_playerNameRecords"("RecordID") ON DELETE SET NULL
);
CREATE TABLE IF NOT EXISTS "tb_playerNameRecords" (
	"RecordID"	INTEGER,
	"MatchID"	INTEGER NOT NULL DEFAULT 0,
	"PlayerName"	TEXT NOT NULL,
	"Date"	TEXT NOT NULL,
	"ExpireDate"	TEXT,
	"AssociatedIP"	INTEGER,
	"RecordType"	INTEGER NOT NULL,
	"RecordCategory"	TEXT NOT NULL,
	"Notes"	TEXT DEFAULT '',
	PRIMARY KEY("RecordID" AUTOINCREMENT),
	FOREIGN KEY("AssociatedIP") REFERENCES "tb_playerIPRecords"("RecordID") ON DELETE SET NULL
);
CREATE TABLE IF NOT EXISTS "tb_proxyBlockedCountries" (
	"RecordID"	INTEGER,
	"CountryCode"	TEXT NOT NULL,
	"CountryName"	TEXT NOT NULL,
	UNIQUE("CountryCode"),
	PRIMARY KEY("RecordID" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "tb_proxyRecords" (
	"RecordID"	INTEGER,
	"IPAddress"	TEXT NOT NULL UNIQUE,
	"IsVpn"	INTEGER NOT NULL DEFAULT 0,
	"IsProxy"	INTEGER NOT NULL DEFAULT 0,
	"IsTor"	INTEGER NOT NULL DEFAULT 0,
	"RiskScore"	INTEGER NOT NULL DEFAULT 0,
	"Provider"	TEXT DEFAULT '',
	"CountryCode"	TEXT DEFAULT '',
	"City"	TEXT DEFAULT '',
	"Region"	TEXT DEFAULT '',
	"CacheExpiry"	TEXT NOT NULL,
	"LastChecked"	TEXT NOT NULL,
	PRIMARY KEY("RecordID" AUTOINCREMENT),
	CHECK("RiskScore" >= 0 AND "RiskScore" <= 100)
);
CREATE TABLE IF NOT EXISTS "tb_settings" (
	"id"	INTEGER,
	"key"	TEXT UNIQUE,
	"value"	TEXT,
	PRIMARY KEY("id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "tb_userPermissions" (
	"PermissionID"	INTEGER,
	"UserID"	INTEGER NOT NULL,
	"Permission"	TEXT NOT NULL,
	PRIMARY KEY("PermissionID" AUTOINCREMENT),
	FOREIGN KEY("UserID") REFERENCES "tb_users"("UserID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "tb_users" (
	"UserID"	INTEGER,
	"Username"	TEXT NOT NULL UNIQUE COLLATE NOCASE,
	"PasswordHash"	TEXT NOT NULL,
	"Salt"	TEXT NOT NULL,
	"IsActive"	INTEGER DEFAULT 1,
	"Created"	TEXT DEFAULT (datetime('now')),
	"LastLogin"	TEXT,
	"Notes"	TEXT DEFAULT '',
	PRIMARY KEY("UserID" AUTOINCREMENT)
);
INSERT INTO "tb_defaultMaps" VALUES (1,'Road Rage','DMK_01A.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (2,'City Madness','DMM_01A.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (3,'Cracked','DMM_01E.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (4,'Walled In','DMM_01G.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (5,'Mines Eye','DMM_01H.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (6,'Stadium Riot','DMM_01K.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (7,'Desert Funeral','DMM_02A.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (8,'Devil Dogs','DMM_03A.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (9,'Squirrels Nest','DMP_01A.BMS',0,0);
INSERT INTO "tb_defaultMaps" VALUES (10,'Culture Clash A','TDMK_01A.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (11,'Culture Clash B','TDMK_01B.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (12,'Mean Streets A','TDMM_01A.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (13,'Mean Streets B','TDMM_01B.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (14,'Panic Attack A','TDMM_01E.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (15,'Panic Attack B','TDMM_01F.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (16,'Rampage A','TDMM_01G.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (17,'Rampage B','TDMM_01H.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (18,'Double Barrel A','TDMM_01K.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (19,'Double Barrel B','TDMM_01L.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (20,'Hornets Nest A','TDMM_02A.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (21,'Hornets Nest B','TDMM_02B.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (22,'Sidewinder A','TDMM_03A.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (23,'Sidewinder B','TDMM_03B.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (24,'Clue Map A','TDMP_01A.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (25,'Clue Map B','TDMP_01B.BMS',0,1);
INSERT INTO "tb_defaultMaps" VALUES (26,'Meat Grinder A','TKHK_01A.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (27,'Mean Grinder B','TKHK_01B.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (28,'House of Pain A','TKHM_01A.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (29,'House of Pain B','TKHM_01B.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (30,'Eye of the Dead A','TKHM_01E.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (31,'Eye of the Dead B','TKHM_01F.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (32,'Dust Devil A','TKHM_01G.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (33,'Dust Devil B','TKHM_01H.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (34,'Spider Web A','TKHM_01K.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (35,'Spider Web B','TKHM_01L.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (36,'Desert Insertion A','TKHM_02A.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (37,'Desert Insertion A','TKHM_02B.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (38,'Desert Fox A','TKHM_03A.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (39,'Desert Fox B','TKHM_03B.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (40,'Tunnel Trouble A','TKHP_01A.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (41,'Tunnel Trouble B','TKHP_01B.BMS',0,3);
INSERT INTO "tb_defaultMaps" VALUES (42,'Mog Mayhem A','SDK_01A.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (43,'Mog Mayhem B','SDK_01B.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (44,'Firefighter A','SDM_01A.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (45,'Firefighter B','SDM_01B.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (46,'Roof Stalker A','SDM_01E.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (47,'Roof Stalker B','SDM_01F.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (48,'Savannah Town A','SDM_01G.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (49,'Savannah Town B','SDM_01H.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (50,'Stadium Feud A','SDM_01K.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (51,'Stadium Feud B','SDM_01L.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (52,'Sky Raiders A','SDM_02A.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (53,'Sky Raiders B','SDM_02B.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (54,'Trail Blazer A','SDM_03A.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (55,'Trail Blazer B','SDM_03B.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (56,'The Hidden A','SDP_01A.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (57,'The Hidden B','SDP_01B.BMS',0,5);
INSERT INTO "tb_defaultMaps" VALUES (58,'Crossfire A','ADK_01A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (59,'Crossfire B','ADK_01B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (60,'Urban Raid A','ADK_02A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (61,'Urban Raid B','ADK_02B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (62,'Rapid Fire A','ADM_01A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (63,'Rapid Fire B','ADM_01B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (64,'Ground Fire A','ADM_01E.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (65,'Ground Fire B','ADM_01F.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (66,'Dust and Bones A','ADM_01G.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (67,'Dust and Bones B','ADM_01H.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (68,'Valkyrie A','ADM_02A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (69,'Valkyrie B','ADM_02B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (70,'Snake Pit A','ADM_03A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (71,'Snake Pit B','ADM_03B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (72,'Lost and Dead A','ADP_01A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (73,'Lost and Dead B','ADP_01B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (74,'Defensive Assault A','ADP_11A.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (75,'Defensive Assault B','ADP_11B.BMS',0,6);
INSERT INTO "tb_defaultMaps" VALUES (76,'The Barrens A','CTFK_01A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (77,'The Barrens B','CTFK_01B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (78,'Tug O War A','CTFK_02A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (79,'Tug O War B','CTFK_02B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (80,'Groundhog Day A','CTFK_03A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (81,'Groundhog Day B','CTFK_03B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (82,'Block Mayhem A','CTFM_01A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (83,'Block Mayhem B','CTFM_01B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (84,'Burned Asphalt A','CTFM_01E.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (85,'Burned Asphalt B','CTFM_01F.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (86,'Desert Trap A','CTFM_01G.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (87,'Desert Trap B','CTFM_01H.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (88,'Snake Eyes A','CTFM_01I.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (89,'Snake Eyes B','CTFM_01J.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (90,'Storage Space A','CTFM_01K.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (91,'Storage Space B','CTFM_01L.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (92,'Dead Zone A','CTFM_02A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (93,'Dead Zone B','CTFM_02B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (94,'Hit and Run A','CTFM_03A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (95,'Hit and Run B','CTFM_03B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (96,'Bad Juju A','CTFM_05A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (97,'Bad Juju B','CTFM_05B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (98,'Ribcage A','CTFP_01A.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (99,'Ribcage B','CTFP_01B.BMS',0,7);
INSERT INTO "tb_defaultMaps" VALUES (100,'Scrimmage Line A','FBK_01A.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (101,'Scrimmage Line B','FBK_01B.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (102,'Blitzkrig A','FBK_03A.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (103,'Blitzkrig B','FBK_03B.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (104,'Pigskin A','FBM_01A.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (105,'Pigskin B','FBM_01B.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (106,'Wall Jumper A','FBM_01E.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (107,'Wall Jumper B','FBM_01F.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (108,'Desert Scarab A','FBM_01G.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (109,'Desert Scarab B','FBM_01H.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (110,'Bloodball A','FBM_01K.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (111,'Bloodball B','FBM_01L.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (112,'Sky Fire A','FBM_02A.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (113,'Sky Fire B','FBM_02B.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (114,'Frontal Assult A','FBM_03A.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (115,'Frontal Assult B','FBM_03B.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (116,'Frantic Traffic A','FBP_01A.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (117,'Frantic Traffic B','FBP_01B.BMS',0,8);
INSERT INTO "tb_defaultMaps" VALUES (118,'Meat Grinder A','KHM_01A.BMS',0,4);
INSERT INTO "tb_defaultMaps" VALUES (119,'Eye of the Dead A','KHM_01E.BMS',0,4);
INSERT INTO "tb_defaultMaps" VALUES (120,'Dust Devil A','KHM_01G.BMS',0,4);
INSERT INTO "tb_defaultMaps" VALUES (121,'Desert Insertion A','KHM_02A.BMS',0,4);
INSERT INTO "tb_defaultMaps" VALUES (122,'Desert Fox A','KHM_03A.BMS',0,4);
CREATE INDEX IF NOT EXISTS "idx_chatLogs_timestamp" ON "tb_chatLogs" (
	"messageTimeStamp"
);
CREATE INDEX IF NOT EXISTS "idx_permissions_permission" ON "tb_userPermissions" (
	"Permission"
);
CREATE INDEX IF NOT EXISTS "idx_permissions_userid" ON "tb_userPermissions" (
	"UserID"
);
CREATE INDEX IF NOT EXISTS "idx_playerip_category" ON "tb_playerIPRecords" (
	"RecordCategory",
	"RecordType"
);
CREATE INDEX IF NOT EXISTS "idx_playerip_expiry" ON "tb_playerIPRecords" (
	"ExpireDate"
);
CREATE INDEX IF NOT EXISTS "idx_playerip_ip" ON "tb_playerIPRecords" (
	"PlayerIP"
);
CREATE INDEX IF NOT EXISTS "idx_playerip_matchid" ON "tb_playerIPRecords" (
	"MatchID"
);
CREATE INDEX IF NOT EXISTS "idx_playername_category" ON "tb_playerNameRecords" (
	"RecordCategory",
	"RecordType"
);
CREATE INDEX IF NOT EXISTS "idx_playername_expiry" ON "tb_playerNameRecords" (
	"ExpireDate"
);
CREATE INDEX IF NOT EXISTS "idx_playername_matchid" ON "tb_playerNameRecords" (
	"MatchID"
);
CREATE INDEX IF NOT EXISTS "idx_playername_name" ON "tb_playerNameRecords" (
	"PlayerName"
);
CREATE INDEX IF NOT EXISTS "idx_proxy_country_code" ON "tb_proxyBlockedCountries" (
	"CountryCode"
);
CREATE INDEX IF NOT EXISTS "idx_proxy_expiry" ON "tb_proxyRecords" (
	"CacheExpiry"
);
CREATE INDEX IF NOT EXISTS "idx_proxy_ip" ON "tb_proxyRecords" (
	"IPAddress"
);
CREATE INDEX IF NOT EXISTS "idx_proxy_vpn" ON "tb_proxyRecords" (
	"IsVpn",
	"IsProxy",
	"IsTor"
);
CREATE INDEX IF NOT EXISTS "idx_users_active" ON "tb_users" (
	"IsActive"
);
CREATE INDEX IF NOT EXISTS "idx_users_username" ON "tb_users" (
	"Username"
);
COMMIT;
