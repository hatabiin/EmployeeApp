# データベーステーブル設計

## COMPANIESテーブル

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| id | INT | PRIMARY KEY, AUTO_INCREMENT | 会社ID |
| company_name | VARCHAR(200) | NOT NULL | 会社名 |
| created_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP | 作成日時 |
| updated_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP | 更新日時 |

## DEPARTMENTSテーブル

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| id | INT | PRIMARY KEY, AUTO_INCREMENT | 部署ID |
| company_id | INT | NOT NULL, FK | 会社ID（外部キー） |
| department_name | VARCHAR(200) | NOT NULL | 部署名 |
| created_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP | 作成日時 |
| updated_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP | 更新日時 |

## DIVISIONSテーブル

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| id | INT | PRIMARY KEY, AUTO_INCREMENT | 課ID |
| company_id | INT | NOT NULL, FK | 会社ID（外部キー） |
| department_id | INT | NOT NULL, FK | 部署ID（外部キー） |
| division_name | VARCHAR(200) | NOT NULL | 課名 |
| created_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP | 作成日時 |
| updated_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP | 更新日時 |

## EMPLOYEESテーブル

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| id | INT | PRIMARY KEY, AUTO_INCREMENT | 社員ID |
| employee_name | VARCHAR(200) | NOT NULL | 社員名 |
| created_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP | 作成日時 |
| updated_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP | 更新日時 |

## LICENSESテーブル

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| id | INT | PRIMARY KEY, AUTO_INCREMENT | 資格ID |
| license_name | VARCHAR(200) | NOT NULL | 資格名 |
| created_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP | 作成日時 |
| updated_at | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP | 更新日時 |

## DEPARTMENT_ASSIGNMENTSテーブル（中間テーブル）

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| employee_id | INT | PK(複合), NOT NULL FK | 社員ID |
| department_id | INT | PK(複合), NOT NULL FK | 部署ID |

## DIVISION_ASSIGNMENTSテーブル（中間テーブル）

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| employee_id | INT | PK(複合), NOT NULL FK | 社員ID |
| division_id | INT | PK(複合), NOT NULL FK | 課ID |

## HOLDINGSテーブル（中間テーブル）

| カラム名 | データ型 | 制約 | 説明 |
|----------|----------|------|------|
| employee_id | INT | PK(複合), NOT NULL, FK | 社員ID |
| license_id | INT | PK(複合), NOT NULL, FK | 資格ID |
