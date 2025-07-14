using EmployeeApp.Data;
using EmployeeApp.Models;

namespace EmployeeApp.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("=== 15人版シードデータ開始 ===");
                
                context.Database.EnsureCreated();
                Console.WriteLine("データベース確認完了");

                // 既にデータがある場合はスキップ（今は無効化）
                // if (context.Employees.Any())
                // {
                //     Console.WriteLine("既にデータが存在するため、シードをスキップします");
                //     return;
                // }

                Console.WriteLine("全パターン対応シードデータを開始します...");

                // 会社データ
                Console.WriteLine("会社データを追加中...");
                var company = new Company { CompanyName = "テックソリューション株式会社" };
                context.Companies.Add(company);
                context.SaveChanges();
                Console.WriteLine($"会社データ追加完了（ID: {company.Id}）");

                // 部署データ（5部署）
                Console.WriteLine("部署データを追加中...");
                var sales = new Department { CompanyId = company.Id, DepartmentName = "営業部" };
                var dev = new Department { CompanyId = company.Id, DepartmentName = "開発部" };
                var admin = new Department { CompanyId = company.Id, DepartmentName = "総務部" };
                var planning = new Department { CompanyId = company.Id, DepartmentName = "企画部" };
                var quality = new Department { CompanyId = company.Id, DepartmentName = "品質管理部" };

                context.Departments.AddRange(sales, dev, admin, planning, quality);
                context.SaveChanges();
                Console.WriteLine($"部署データ追加完了（5部署）");

                // 課データ（10課）
                Console.WriteLine("課データを追加中...");
                var sales1 = new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "営業1課" };
                var sales2 = new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "営業2課" };
                var overseas = new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "海外営業課" };
                var system = new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "システム開発課" };
                var web = new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "Web開発課" };
                var mobile = new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "モバイル開発課" };
                var hr = new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "人事課" };
                var accounting = new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "経理課" };
                var product = new Division { CompanyId = company.Id, DepartmentId = planning.Id, DivisionName = "商品企画課" };
                var qa = new Division { CompanyId = company.Id, DepartmentId = quality.Id, DivisionName = "品質保証課" };

                context.Divisions.AddRange(sales1, sales2, overseas, system, web, mobile, hr, accounting, product, qa);
                context.SaveChanges();
                Console.WriteLine($"課データ追加完了（10課）");

                // 資格データ（8資格）
                Console.WriteLine("資格データを追加中...");
                var basicIT = new License { LicenseName = "基本情報技術者" };
                var advancedIT = new License { LicenseName = "応用情報技術者" };
                var boki2 = new License { LicenseName = "簿記2級" };
                var boki1 = new License { LicenseName = "簿記1級" };
                var toeic800 = new License { LicenseName = "TOEIC800点以上" };
                var toeic900 = new License { LicenseName = "TOEIC900点以上" };
                var license = new License { LicenseName = "普通自動車免許" };
                var pmp = new License { LicenseName = "PMP(プロジェクトマネジメント)" };

                context.Licenses.AddRange(basicIT, advancedIT, boki2, boki1, toeic800, toeic900, license, pmp);
                context.SaveChanges();
                Console.WriteLine($"資格データ追加完了（8資格）");

                // 社員データ（15人 - 全パターン網羅）
                Console.WriteLine("社員データを追加中...");
                var emp1 = new Employee { EmployeeName = "田中太郎", PasswordHash = "test123" };      // マネージャー（複数部署兼務）
                var emp2 = new Employee { EmployeeName = "佐藤花子", PasswordHash = "test123" };      // 営業エース
                var emp3 = new Employee { EmployeeName = "鈴木次郎", PasswordHash = "test123" };      // フルスタック開発者
                var emp4 = new Employee { EmployeeName = "高橋美香", PasswordHash = "test123" };      // 人事・総務兼務
                var emp5 = new Employee { EmployeeName = "渡辺健", PasswordHash = "test123" };        // システム開発リーダー
                var emp6 = new Employee { EmployeeName = "山田愛", PasswordHash = "test123" };        // 海外営業担当
                var emp7 = new Employee { EmployeeName = "中村大輔", PasswordHash = "test123" };      // 企画・品質管理兼務
                var emp8 = new Employee { EmployeeName = "小林さくら", PasswordHash = "test123" };    // Web・モバイル開発
                var emp9 = new Employee { EmployeeName = "加藤正", PasswordHash = "test123" };        // 経理専門
                var emp10 = new Employee { EmployeeName = "松本優子", PasswordHash = "test123" };     // 営業・企画兼務
                // 特殊パターン追加
                var emp11 = new Employee { EmployeeName = "伊藤直樹", PasswordHash = "test123" };     // 部署のみ所属（課なし）
                var emp12 = new Employee { EmployeeName = "木村真理", PasswordHash = "test123" };     // 課のみ所属（部署なし）
                var emp13 = new Employee { EmployeeName = "斎藤和也", PasswordHash = "test123" };     // 資格なし
                var emp14 = new Employee { EmployeeName = "新人田辺", PasswordHash = "test123" };     // 完全未配属（資格あり）
                var emp15 = new Employee { EmployeeName = "研修生山本", PasswordHash = "test123" };   // 完全未配属（資格なし）

                context.Employees.AddRange(emp1, emp2, emp3, emp4, emp5, emp6, emp7, emp8, emp9, emp10, emp11, emp12, emp13, emp14, emp15);
                context.SaveChanges();
                Console.WriteLine($"社員データ追加完了（15人）");

                // 関連付け開始
                Console.WriteLine("関連付けを追加中...");

                // 田中太郎：マネージャー（営業部・開発部兼務、複数課、多資格）
                emp1.Departments.Add(sales);
                emp1.Departments.Add(dev);
                emp1.Divisions.Add(sales1);
                emp1.Divisions.Add(system);
                emp1.Licenses.Add(basicIT);
                emp1.Licenses.Add(advancedIT);
                emp1.Licenses.Add(pmp);
                emp1.Licenses.Add(license);

                // 佐藤花子：営業エース（営業部、複数課、英語資格）
                emp2.Departments.Add(sales);
                emp2.Divisions.Add(sales1);
                emp2.Divisions.Add(overseas);
                emp2.Licenses.Add(toeic900);
                emp2.Licenses.Add(boki2);
                emp2.Licenses.Add(license);

                // 鈴木次郎：フルスタック開発者（開発部、全開発課、IT資格フル）
                emp3.Departments.Add(dev);
                emp3.Divisions.Add(system);
                emp3.Divisions.Add(web);
                emp3.Divisions.Add(mobile);
                emp3.Licenses.Add(basicIT);
                emp3.Licenses.Add(advancedIT);

                // 高橋美香：人事・総務兼務（総務部、複数課）
                emp4.Departments.Add(admin);
                emp4.Divisions.Add(hr);
                emp4.Divisions.Add(accounting);
                emp4.Licenses.Add(boki2);
                emp4.Licenses.Add(toeic800);

                // 渡辺健：システム開発リーダー（開発部・品質管理部兼務）
                emp5.Departments.Add(dev);
                emp5.Departments.Add(quality);
                emp5.Divisions.Add(system);
                emp5.Divisions.Add(qa);
                emp5.Licenses.Add(basicIT);
                emp5.Licenses.Add(pmp);

                // 山田愛：海外営業担当（営業部、英語スペシャリスト）
                emp6.Departments.Add(sales);
                emp6.Divisions.Add(overseas);
                emp6.Licenses.Add(toeic900);
                emp6.Licenses.Add(license);

                // 中村大輔：企画・品質管理兼務（複数部署）
                emp7.Departments.Add(planning);
                emp7.Departments.Add(quality);
                emp7.Divisions.Add(product);
                emp7.Divisions.Add(qa);
                emp7.Licenses.Add(basicIT);
                emp7.Licenses.Add(pmp);

                // 小林さくら：Web・モバイル開発（開発部、複数課）
                emp8.Departments.Add(dev);
                emp8.Divisions.Add(web);
                emp8.Divisions.Add(mobile);
                emp8.Licenses.Add(basicIT);

                // 加藤正：経理専門（総務部、経理のプロ）
                emp9.Departments.Add(admin);
                emp9.Divisions.Add(accounting);
                emp9.Licenses.Add(boki1);
                emp9.Licenses.Add(boki2);

                // 松本優子：営業・企画兼務（複数部署、複数課）
                emp10.Departments.Add(sales);
                emp10.Departments.Add(planning);
                emp10.Divisions.Add(sales2);
                emp10.Divisions.Add(product);
                emp10.Licenses.Add(toeic800);
                emp10.Licenses.Add(boki2);

                // ========== 特殊パターンの関連付け ==========

                // 伊藤直樹：部署のみ所属（課に所属しない）
                emp11.Departments.Add(dev);  // 開発部のみ
                emp11.Licenses.Add(basicIT);

                // 木村真理：課のみ所属（部署に所属しない）
                emp12.Divisions.Add(hr);  // 人事課のみ
                emp12.Licenses.Add(toeic800);

                // 斎藤和也：正常な所属だが資格なし
                emp13.Departments.Add(sales);
                emp13.Divisions.Add(sales1);
                // 資格は意図的に追加しない

                // 新人田辺：完全未配属（部署も課もなし）だが資格あり
                emp14.Licenses.Add(license);  // 運転免許のみ
                // 部署・課は意図的に追加しない

                // 研修生山本：完全未配属（部署・課・資格すべてなし）
                // 何も追加しない（本当に空っぽの状態）

                context.SaveChanges();
                Console.WriteLine("関連付け追加完了");

                Console.WriteLine("=== 15人版シードデータ完了 ===");
                Console.WriteLine("パターン別内訳:");
                Console.WriteLine("- 複数部署兼務: 田中、渡辺、中村、松本");
                Console.WriteLine("- 複数課担当: 佐藤、鈴木、高橋、小林等");
                Console.WriteLine("- 部署のみ所属: 伊藤直樹");
                Console.WriteLine("- 課のみ所属: 木村真理");
                Console.WriteLine("- 資格なし: 斎藤和也");
                Console.WriteLine("- 完全未配属(資格あり): 新人田辺");
                Console.WriteLine("- 完全未配属(資格なし): 研修生山本");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== シードデータエラー ===");
                Console.WriteLine($"エラー: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"内部エラー: {ex.InnerException.Message}");
                }
                Console.WriteLine($"詳細: {ex.StackTrace}");
                throw;
            }
        }
    }
}