# ベースイメージ: .NET 8 SDK（アプリをビルドするため）
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 作業ディレクトリを設定
WORKDIR /src

# プロジェクトファイルをコピー
COPY EmployeeApp.csproj .

# NuGetパッケージを復元
RUN dotnet restore

# ソースコードをすべてコピー
COPY . .

# アプリケーションをビルド
RUN dotnet build -c Release -o /app/build

# アプリケーションを発行（パブリッシュ）
RUN dotnet publish -c Release -o /app/publish

# 実行用イメージ: .NET 8 Runtime（軽量版）
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# 作業ディレクトリを設定
WORKDIR /app

# ビルド済みファイルをコピー
COPY --from=build /app/publish .

# ポート5000を公開
EXPOSE 5000

# アプリケーション起動コマンド
ENTRYPOINT ["dotnet", "EmployeeApp.dll"]