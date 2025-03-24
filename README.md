## Robocode?
Robocode adalah platform pemrograman membuat robot untuk mencari poin tertinggi. Disini, kita belajar implementasi strategi algoritma greedy untuk meraih poin tertinggi, melawan terhadap orang lain.

## Botnya apa aja sih?
1. Suniper2 (main)
- Bot ini bertujuan untuk menembak musuh dari jauh. Apabila ada yang mendekat, dia akan berusaha untuk menjauh agar bisa bertahan hidup selama mungkin.
2. NearWall (alt)
- Bot NearWall akan memberi dirinya jarak terhadap dinding, kemudian memutari dinding sambil menjaga jarak itu. Apabila ketemu musuh, langsung menembak
3. CircleImpact (alt)
- Bot ini adalah bot yang mencari oportunitas, dia akan berusaha bertahan hidup, menunggu musuh menjadi lebih sedikit, sebelum menarget musuh.
4. TRIPIN (alt)
- Bot ini berputar dengan pola segitiga dengan tiap sudutnya membentuk bola. Tujuannya adalah untuk membuat peluru lawan lebih sulit kena.


## Instalasi
1. Clone repositori ini:
    ```bash
    git clone https://github.com/username/Tubes1_Sucipto.git
    ```
2. Masuk ke direktori proyek:
    ```bash
    cd Tubes1_Sucipto
    ```
3. Pastikan Anda telah menginstal Java dan .NET di sistem Anda:
    - **Java**: Unduh dan instal Java Development Kit (JDK) dari [situs resmi Oracle](https://www.oracle.com/java/technologies/javase-downloads.html) atau gunakan distribusi open-source seperti OpenJDK.
    - **.NET**: Unduh dan instal .NET SDK dari [situs resmi Microsoft](https://dotnet.microsoft.com/download).

## Cara Penggunaan
1. Clone direktori, lalu lakukan run:
```bash
    java -jar robocode-tankroyale-gui-0.30.0.jar
```
2. Di dalam program, tekan config, lalu tambahkan direktori ke lokasi folder bot yang siap digunakan
3. Tekan Battle, lalu boot program bot yang ingin digunakan
4. Tambahkan bot yang ingin digunakan dari bot yang telah diboot
5. Tekan start "Start Battle"

## Struktur Direktori
```
Tubes1_Sucipto/
├── src/              # Kode sumber
├── sniper2/          # main-bot
├── alternative-bot/
├───── NearWall/      # alt-bot 1
├───── TRIPIN/        # alt-bot 2
├───── CircleImpact/  # alt-bot 3
├── docs/             # dokumen
└── README.md         # readme
```



## Dibuat oleh:
1. Rafen Max Alessandro / 13523031
2. Aloisius Adrian Stevan Gunawan / 13523054