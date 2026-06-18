# UTSVR - Control Room Audio Simulator

Sebuah proyek Unity 6 yang mensimulasikan ruang kontrol interaktif berbasis first-person. Pemain dapat berjalan di dalam sebuah ruangan 3D dan mengatur pengaturan audio (BGM dan SFX) melalui panel kontrol yang berada di dalam dunia permainan.

## Tentang Proyek

Proyek ini membangun sebuah environment "Control Room" di mana pemain bisa:

- Menjelajahi ruangan 3D dengan dinding, lantai, dan meja kontrol
- Bergerak menggunakan kontrol first-person (WASD + mouse look + lompat)
- Berinteraksi dengan panel audio yang dirender sebagai World Space Canvas
- Mengatur volume BGM dan SFX secara terpisah melalui slider
- Membisukan (mute) setiap channel audio secara independen
- Menguji efek suara langsung dari panel
- Mengatur ulang semua pengaturan audio ke nilai default

## Teknologi

| Komponen | Versi/Keterangan |
|----------|-----------------|
| Unity | 6000.4.6f1 (Unity 6) |
| Render Pipeline | Universal Render Pipeline (URP) 17.4.0 |
| Bahasa | C# |
| Template | com.unity.template.urp-blank |
| Input System | Unity Input System 1.19.0 |
| Text | TextMeshPro |

## Struktur Proyek

```
Assets/
  Scenes/
    ControlRoom.unity          -- Scene utama
  Script/
    FirstPersonController.cs   -- Kontroler gerak first-person
    AudioSettings.cs           -- Kontroler panel pengaturan audio
    Mainmixer.mixer            -- Audio mixer (Master group)
  Audio/
    25. Main Theme.mp3         -- Musik latar
    54. Sfx - Bird 01 Flying.mp3  -- Efek suara
    hidup-j.mp3                -- Klip audio tambahan
  Settings/
    PC_RPAsset.asset           -- Pipeline render untuk PC
    Mobile_RPAsset.asset       -- Pipeline render untuk Mobile
```

## Cara Kerja

### Scene ControlRoom

Scene utama terdiri dari beberapa komponen utama:

- **Player** -- Objek pemain dengan FirstPersonController dan CharacterController
- **Main Camera** -- Kamera yang mengikuti pemain (child dari Player)
- **Floor, Walls** -- Geometri ruangan (lantai dan dinding)
- **ControlDesk** -- Meja kontrol di dalam ruangan
- **AudioManager** -- Objek yang mengelola semua audio (BGM dan SFX) melalui script AudioSettings
- **AudioPanel** -- World Space Canvas yang berisi slider, toggle, tombol, dan label persentase
- **Directional Light** -- Pencahayaan utama scene

### Kontrol First-Person (FirstPersonController.cs)

Script ini menggunakan Unity CharacterController untuk menggerakkan pemain:

- **WASD** -- Bergerak relatif terhadap arah kamera
- **Mouse** -- Menggerakkan kamera (pitch dibatasi sampai 85 derajat)
- **Space** -- Melompat dengan ketinggian yang bisa dikonfigurasi
- **Escape** -- Beralih antara mode bermain dan mode UI

Ketika mode UI aktif, kursor dibebaskan dan pergerakan dihentikan sehingga pemain bisa berinteraksi dengan panel audio. Gravitasi tetap berjalan agar pemain tidak melayang.

### Panel Audio (AudioSettings.cs)

Panel audio dirender sebagai World Space Canvas yang terlihat di dalam dunia 3D. Fitur-fiturnya:

- **Slider BGM** -- Mengatur volume musik latar (0-100%)
- **Slider SFX** -- Mengatur volume efek suara (0-100%)
- **Toggle BGM** -- Membisukan/mengaktifkan musik latar
- **Toggle SFX** -- Membisukan/mengaktifkan efek suara
- **Tombol Test SFX** -- Memainkan efek suara untuk pengujian
- **Tombol Reset** -- Mengembalikan semua pengaturan ke nilai awal
- **Label Persentase** -- Menampilkan level volume saat ini secara real-time

Ketika channel dibisukan, label berubah warna menjadi merah dan slider dinonaktifkan.

## Persyaratan

- Unity 6000.4.6f1 atau lebih baru
- Paket Universal Render Pipeline (URP)
- Paket TextMeshPro
- Paket Unity Input System

## Cara Menjalankan

1. Clone repository ini:
   ```
   git clone https://github.com/KenonymouszzZ/UTSVR.git
   ```
2. Buka proyek menggunakan Unity 6 (versi 6000.4.6f1 atau kompatibel)
3. Buka scene `Assets/Scenes/ControlRoom.unity`
4. Tekan tombol Play di Unity Editor
5. Gunakan WASD untuk bergerak, mouse untuk melihat, Space untuk melompat
6. Tekan Escape untuk membuka mode UI dan berinteraksi dengan panel audio

## Catatan Pengembangan

- Kode kontroler menggunakan API `Input.GetAxis` (legacy) alih-alih Unity Input System yang baru, meskipun package Input System sudah terpasang
- Daftar build di `EditorBuildSettings.asset` masih merujuk ke `SampleScene.unity`, bukan `ControlRoom.unity` -- perlu diperbarui sebelum membangun executable
- Quality settings dikonfigurasi untuk dua tier: Mobile dan PC, masing-masing dengan pipeline render URP yang terpisah
