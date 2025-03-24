## Robocode

Robocode adalah platform pemrograman untuk membuat robot yang bertujuan meraih poin tertinggi. Di sini, kita belajar mengimplementasikan strategi algoritma greedy untuk mendapatkan poin tertinggi dan bersaing dengan pemain lain.

## Bot yang Tersedia

1. **Suniper2 (main)**  
    Bot ini dirancang untuk menembak musuh dari jarak jauh. Jika ada musuh yang mendekat, bot ini akan berusaha menjauh untuk bertahan hidup selama mungkin.

2. **NearWall (alt)**  
    Bot ini menjaga jarak dengan dinding dan memutari dinding sambil mempertahankan jarak tersebut. Jika bertemu musuh, bot ini akan langsung menembak.

3. **CircleImpact (alt)**  
    Bot ini mencari peluang dengan bertahan hidup dan menunggu jumlah musuh berkurang sebelum mulai menargetkan musuh.

4. **TRIPIN (alt)**  
    Bot ini bergerak dengan pola segitiga, membentuk bola di setiap sudutnya. Tujuannya adalah membuat peluru lawan lebih sulit mengenainya.

## Instalasi

Pastikan Anda telah menginstal Java dan .NET di sistem Anda:

- **Java**: Unduh dan instal Java Development Kit (JDK) dari [situs resmi Oracle](https://www.oracle.com/java/technologies/javase-downloads.html) atau gunakan distribusi open-source seperti OpenJDK.
- **.NET**: Unduh dan instal .NET SDK dari [situs resmi Microsoft](https://dotnet.microsoft.com/download). **Pastikan menggunakan versi .NET SDK 6.**

## Cara Penggunaan

1. Unduh file .jar dari rilis terbaru di repository GitHub berikut:  
    [Repository Github](https://github.com/Ariel-HS/tubes1-if2211-starter-pack/releases/tag/v1.0)

2. Unduh aset dari rilis terbaru di repository GitHub Sucipto.

3. Jalankan perintah berikut di terminal setelah memastikan Anda berada di direktori yang berisi file .jar:  
    ```bash
    java -jar robocode-tankroyale-gui-0.30.0.jar
    ```

    Jika file .jar tidak ada di direktori tersebut, gunakan perintah berikut untuk berpindah direktori (tanpa tanda petik):  
    ```bash
    cd "path"
    ```

4. Konfigurasikan booter dengan menekan **Config**, lalu **Bot Root Directories**, dan masukkan path ke folder yang berisi folder bot yang ingin digunakan.

5. Tekan **Battle**, lalu boot program bot yang ingin digunakan.

6. Tambahkan bot yang ingin digunakan dari daftar bot yang telah diboot.

7. Jika diperlukan, ubah peraturan dengan menggunakan **Setup Rules**.

8. Tekan **Start Battle** untuk memulai.

## Troubleshooting

Jika file tidak dapat di-boot, lakukan langkah berikut:

1. Pastikan .NET yang diunduh memiliki versi 6. Periksa dengan mengetikkan perintah berikut di terminal:  
    ```bash
    dotnet --version
    ```

2. Pastikan Java sudah terinstal. Periksa dengan mengetikkan perintah berikut di terminal:  
    ```bash
    java --version
    ```

3. Re-compile bot:  
    - Buka direktori folder bot, lalu ketikkan perintah berikut di terminal:  
      ```bash
      dotnet clean
      ```
    - Setelah itu, lakukan re-build dengan mengetikkan:  
      ```bash
      dotnet build
      ```

## Dibuat oleh

1. Rafen Max Alessandro / 13523031  
2. Aloisius Adrian Stevan Gunawan / 13523054