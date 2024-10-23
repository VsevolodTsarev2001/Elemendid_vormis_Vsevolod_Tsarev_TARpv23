using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Sobitamise_mang : Form
    {
        // Список чисел для карт (каждое число дважды, так как у каждой пары есть одинаковые карты)
        List<int> numbers = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };

        // Переменные для хранения выбранных карт
        string esimeneValik; // Первая выбранная карта
        string teineValik; // Вторая выбранная карта

        // Переменная для подсчета попыток
        int katseid;

        // Список всех объектов PictureBox (каждое изображение)
        List<PictureBox> pildid = new List<PictureBox>();

        // Переменные для первой и второй выбранной картинки
        PictureBox piltA; // Первая выбранная картинка
        PictureBox piltB; // Вторая выбранная картинка

        // Метки на экране для отображения информации
        Label lblStaatus; // Статус игры (например, "Вы выиграли" или "Вы проиграли")
        Label lblAeg; // Оставшееся время игры

        // Таймер для отслеживания времени
        System.Windows.Forms.Timer ManguTaimer;

        // Переменная для общего времени игры (60 секунд)
        int koguAeg = 60;

        // Переменная для отслеживания оставшегося времени
        int loendusAeg;

        // Логическая переменная для проверки, завершена ли игра
        bool mangLabi = false;

        // Переменная для количества подсказок (3 подсказки)
        int vihjeKordadeArv = 3;

        // Кнопка для подсказок
        Button btnVihje;

        // Кнопки "Начать заново" и "Проверить результат"
        Button btnRestart;
        Button btnKontrolliVastuseid;

        // Метка для отображения лучшего результата
        Label lblParimTulemus;

        // Переменная для хранения лучшего времени
        int parimTulemus = int.MaxValue;

        // Конструктор формы игры
        public Sobitamise_mang(int w, int h)
        {
            // Инициализация компонентов формы
            InitializeComponent();

            // Размер окна формы
            this.ClientSize = new Size(480, 650);

            // Цвет фона формы
            this.BackColor = Color.LightBlue;

            // Инициализация таймера и установка интервала на 1 секунду
            ManguTaimer = new System.Windows.Forms.Timer();
            ManguTaimer.Interval = 1000;

            // Связывание события таймера с функцией, которая выполняется каждую секунду
            ManguTaimer.Tick += TaimeriSundmus;

            // Метка для отображения статуса игры
            lblStaatus = new Label
            {
                Location = new Point(20, 520),
                Size = new Size(200, 40),
                ForeColor = Color.DarkGreen,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Mäng on alanud!" // Исходное сообщение
            };

            // Метка для отображения оставшегося времени
            lblAeg = new Label
            {
                Location = new Point(20, 560),
                Size = new Size(200, 40),
                ForeColor = Color.Red,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Järelejäänud aeg: 60" // Исходное время
            };

            // Метка для отображения лучшего результата
            lblParimTulemus = new Label
            {
                Location = new Point(250, 560),
                Size = new Size(200, 40),
                ForeColor = Color.Black,
                Text = "Parim tulemus: --", // В данный момент результата нет
                Font = new Font("Arial", 10, FontStyle.Italic)
            };

            // Добавление меток на форму
            this.Controls.Add(lblStaatus);
            this.Controls.Add(lblAeg);
            this.Controls.Add(lblParimTulemus);

            // Добавление кнопки "Начать заново"
            LisaUuestiNupp();

            // Добавление кнопки для проверки результата
            LisaKontrolliVastuseidNupp();

            // Добавление кнопки для подсказки
            LisaVihjeNupp();

            // Загрузка изображений на игровое поле
            LaadiPildid();
        }

        // Функция для добавления кнопки подсказки
        private void LisaVihjeNupp()
        {
            btnVihje = new Button
            {
                Text = "Vihje (" + vihjeKordadeArv + " jäi)", // Текст, показывающий количество оставшихся подсказок
                Size = new Size(100, 30),
                Location = new Point(320, 480)
            };

            // Связывание события с кнопкой подсказки
            btnVihje.Click += VihjeNupp_Click;

            // Добавление кнопки на форму
            this.Controls.Add(btnVihje);
        }

        // Функция, вызываемая при нажатии на кнопку подсказки
        private void VihjeNupp_Click(object sender, EventArgs e)
        {
            // Если остались подсказки и игра не закончена, показываем подсказку
            if (vihjeKordadeArv > 0 && !mangLabi)
            {
                vihjeKordadeArv--;
                btnVihje.Text = "Vihje (" + vihjeKordadeArv + " jäi)";

                // Показать две карты в качестве подсказки
                // Определите логику, чтобы выбрать два случайных изображения
                var pairedPictures = pildid.Where(x => x.Tag != null).OrderBy(x => Guid.NewGuid()).Take(2).ToList();
                foreach (var picture in pairedPictures)
                {
                    picture.Image = Image.FromFile(@"..\..\..\" + (string)picture.Tag + ".png");
                }
                // Через 2 секунды скрыть подсказку
                System.Windows.Forms.Timer hintTimer = new System.Windows.Forms.Timer();
                hintTimer.Interval = 2000; // 2 секунды
                hintTimer.Tick += (s, args) =>
                {
                    hintTimer.Stop();
                    foreach (var picture in pairedPictures)
                    {
                        picture.Image = null;
                    }
                };
                hintTimer.Start();
            }
        }

        // Функция для обработки события таймера
        private void TaimeriSundmus(object sender, EventArgs e)
        {
            loendusAeg--; // Уменьшаем оставшееся время на 1 секунду
            lblAeg.Text = "Järelejäänud aeg: " + loendusAeg; // Обновляем текст метки времени
            if (loendusAeg < 1) // Если время закончилось
            {
                ManguLopp("Aeg on läbi, sa oled kaotanud"); // Завершение игры
                // Скрываем все карты
                foreach (PictureBox x in pildid)
                {
                    if (x.Tag != null)
                    {
                        x.Image = Image.FromFile(@"..\..\..\" + (string)x.Tag + ".png"); // Показываем все карты
                    }
                }
            }
        }

        // Функция для загрузки изображений на игровое поле
        private void LaadiPildid()
        {
            int vasakPos = 20; // Начальная позиция по оси X
            int yleminePos = 20; // Начальная позиция по оси Y
            int ridu = 0; // Счетчик строк
            for (int i = 0; i < 16; i++)
            {
                PictureBox uusPilt = new PictureBox(); // Создание нового PictureBox
                uusPilt.Height = 100; // Высота картинки
                uusPilt.Width = 100; // Ширина картинки
                uusPilt.BackColor = Color.DarkGreen; // Цвет фона картинки
                uusPilt.SizeMode = PictureBoxSizeMode.Zoom; // Режим отображения
                uusPilt.Click += UusPilt_Click; // Привязка события клика
                pildid.Add(uusPilt); // Добавление картинки в список
                if (ridu < 4) // Если не больше 4-х карт в строке
                {
                    ridu++;
                    uusPilt.Left = vasakPos; // Установка позиции по оси X
                    uusPilt.Top = yleminePos; // Установка позиции по оси Y
                    this.Controls.Add(uusPilt); // Добавление картинки на форму
                    vasakPos += 110; // Сдвиг позиции для следующей картинки
                }
                if (ridu == 4) // Если уже 4 картинки в строке
                {
                    vasakPos = 20; // Сброс позиции по оси X
                    yleminePos += 110; // Сдвиг позиции по оси Y
                    ridu = 0; // Сброс счетчика строк
                }
            }
            RestartMäng(); // Запуск новой игры
        }

        // Обработчик клика по изображению
        private void UusPilt_Click(object sender, EventArgs e)
        {
            if (mangLabi) // Если игра завершена, не реагируем на клики
                return;

            // Проверка первой выбранной картинки
            if (esimeneValik == null)
            {
                piltA = sender as PictureBox; // Сохранение первой выбранной картинки
                if (piltA.Tag != null && piltA.Image == null) // Проверка, есть ли тег и изображение не загружено
                {
                    piltA.Image = Image.FromFile(@"..\..\..\" + (string)piltA.Tag + ".png"); // Загружаем изображение
                    esimeneValik = (string)piltA.Tag; // Сохраняем тег первой картинки
                }
            }
            // Проверка второй выбранной картинки
            else if (teineValik == null)
            {
                piltB = sender as PictureBox; // Сохранение второй выбранной картинки
                if (piltB.Tag != null && piltB.Image == null) // Проверка, есть ли тег и изображение не загружено
                {
                    piltB.Image = Image.FromFile(@"..\..\..\" + (string)piltB.Tag + ".png"); // Загружаем изображение
                    teineValik = (string)piltB.Tag; // Сохраняем тег второй картинки
                }
            }
            // Если обе картинки выбраны, проверяем их
            else
            {
                KontrolliPilti(piltA, piltB); // Вызываем функцию для проверки выбранных карт
            }
        }

        // Функция для перезапуска игры
        private void RestartMäng()
        {
            var juhuslikLoend = numbers.OrderBy(x => Guid.NewGuid()).ToList(); // Перемешиваем числа
            numbers = juhuslikLoend; // Обновляем список чисел
            for (int i = 0; i < pildid.Count; i++)
            {
                pildid[i].Image = null; // Очищаем изображения
                pildid[i].Tag = numbers[i].ToString(); // Устанавливаем тег для каждой картинки
            }
            katseid = 0; // Сбрасываем счетчик попыток
            mangLabi = false; // Сбрасываем состояние игры
            loendusAeg = koguAeg; // Восстанавливаем оставшееся время
            ManguTaimer.Start(); // Запускаем таймер
            lblStaatus.Text = "Mäng on sisse lülitatud!"; // Обновляем статус игры
            lblAeg.Text = "Järelejäänud aeg: " + loendusAeg; // Обновляем метку времени
        }

        // Функция для проверки двух выбранных карт
        private void KontrolliPilti(PictureBox A, PictureBox B)
        {
            if (esimeneValik == teineValik) // Если картинки совпадают
            {
                A.Tag = null; // Удаляем тег первой картинки
                B.Tag = null; // Удаляем тег второй картинки
            }
            else
            {
                katseid++; // Увеличиваем счетчик попыток
            }
            esimeneValik = null; // Сбрасываем выбор первой картинки
            teineValik = null; // Сбрасываем выбор второй картинки
            foreach (PictureBox pilt in pildid.ToList()) // Для всех карт в списке
            {
                if (pilt.Tag != null) // Если тег не равен null
                {
                    pilt.Image = null; // Скрываем картинку
                }
            }
            if (pildid.All(o => o.Tag == null)) // Если все картинки открыты
            {
                ManguLopp("Hea küll, sa võitsid.!!!!"); // Завершение игры с победным сообщением
                // Сохранение лучшего результата
                if (katseid < parimTulemus)
                {
                    parimTulemus = katseid; // Обновляем лучший результат
                    lblParimTulemus.Text = "Parim tulemus: " + parimTulemus; // Обновляем метку лучшего результата
                }
            }
        }

        // Функция для завершения игры
        private void ManguLopp(string msg)
        {
            ManguTaimer.Stop(); // Остановка таймера
            mangLabi = true; // Устанавливаем состояние игры в завершенное
            MessageBox.Show(msg + " Uuesti mängimiseks klõpsake nuppu „Start Over“."); // Показываем сообщение
        }

        // Функция для добавления кнопки проверки результата
        private void LisaKontrolliVastuseidNupp()
        {
            btnKontrolliVastuseid = new Button
            {
                Text = "Kontrollige tulemust",
                Size = new Size(120, 30),
                Location = new Point(50, 480)
            };
            btnKontrolliVastuseid.Click += KontrolliVastuseidNupp_Click; // Привязка события клика
            this.Controls.Add(btnKontrolliVastuseid); // Добавление кнопки на форму
        }

        // Функция для добавления кнопки «Начать заново»
        private void LisaUuestiNupp()
        {
            btnRestart = new Button();
            btnRestart.Text = "Alusta uuesti";
            btnRestart.Size = new Size(100, 30);
            btnRestart.Location = new Point(180, 480);
            btnRestart.Click += btnRestart_Click; // Привязка события клика
            this.Controls.Add(btnRestart); // Добавление кнопки на форму
        }

        // Обработчик клика по кнопке проверки результата
        private void KontrolliVastuseidNupp_Click(object sender, EventArgs e)
        {
            if (pildid.All(o => o.Tag == null)) // Если все картинки открыты
            {
                ManguLopp("Hea küll, sa võitsid.!!!!"); // Завершение игры с победным сообщением
            }
            else // Если остались непарные карты
            {
                MessageBox.Show("On ka paaritu kaardid!", "Tulemuse kontrollimine", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение о непарных картах
            }
        }

        // Обработчик клика по кнопке «Начать заново»
        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartMäng(); // Перезапуск игры
        }
    }
}
