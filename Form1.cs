using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private double startHeight;
        private double startSpeed;
        private double startAngle;

        private PlotModel trajectoryModel;
        private PlotModel speedModel;
        private PlotModel coordModel;

        private PlotView trajectoryView;
        private PlotView speedView;
        private PlotView coordView;

        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button button1;

        private Label textForTB1;
        private Label textForTB2;
        private Label textForTB3;

        public Form1()
        {
            InitializeComponent();
            //текст для боксов
            textForTB1 = new Label { Text = "Высота", Location = new Point(10, 10) };
            Controls.Add(textForTB1);

            textForTB2 = new Label { Text = "Скорость", Location = new Point(10, 70) };
            Controls.Add(textForTB2);

            textForTB2 = new Label { Text = "Угол", Location = new Point(10, 130) };
            Controls.Add(textForTB2);

            //графики
            trajectoryModel = new PlotModel() { Title = "Траектория" };
            speedModel = new PlotModel() { Title = "Скорость" };
            coordModel = new PlotModel() { Title = "Координаты" };

            trajectoryView = new PlotView() { Model = trajectoryModel, Dock = DockStyle.Top, Height = 300 };
            speedView = new PlotView() { Model = speedModel, Dock = DockStyle.Top, Height = 300 };
            coordView = new PlotView() { Model = coordModel, Dock = DockStyle.Top, Height = 300 };


            //боксы и кнопка
            textBox1 = new TextBox { Location = new Point(10, 40), Size = new Size(140, 40), Text = startHeight.ToString() };
            textBox2 = new TextBox { Location = new Point(10, 100), Size = new Size(140, 40), Text = startSpeed.ToString() };
            textBox3 = new TextBox { Location = new Point(10, 160), Size = new Size(140, 40), Text = startAngle.ToString() };
            button1 = new Button { Location = new Point(10, 190), Size = new Size(140, 60), Text = "Рассчитать" };

            textBox1.TextChanged += textBox1_TextChanged;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox3.TextChanged += textBox3_TextChanged;
            button1.Click += button1_Click;
            
            Controls.Add(textBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox3);
            Controls.Add(button1);
            Controls.Add(coordView);
            Controls.Add(speedView);
            Controls.Add(trajectoryView);

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double height))
            {
                startHeight = height;
            }
        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox2.Text, out double speed))
            {
                startSpeed = speed;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox3.Text, out double angle))
            {
                startAngle = angle * Math.PI / 180;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // очистка предыдущих данных
            trajectoryModel.Series.Clear();
            speedModel.Series.Clear();
            coordModel.Series.Clear();
            
            const double g = 9.81; //ускорение
            double time = 0; //время
            //координаты
            double x = 0;
            double y = startHeight;
            //формулы
            double vx = startSpeed * Math.Cos(startAngle);
            double vy = startSpeed * Math.Sin(startAngle);

            var trajectorySeries = new LineSeries { Title = "Траектория", Color = OxyColors.Green};

            while (y >= 0)
            {
                // расчет координат
                x = vx * time;
                y = startHeight + vy * time - 0.5 * g * time * time;
                trajectorySeries.Points.Add(new DataPoint(x, y));
              
                time += 0.1;
            }
            trajectoryModel.Series.Add(trajectorySeries);
            double finalTime = time; // сохранить последнее время
            
            // Скорости
            var totalSpeed = new LineSeries { Title = "V", Color = OxyColors.Orange };

            for (double t = 0; t <= finalTime; t += 0.1)
            {
                double formulaForV = Math.Sqrt(vx * vx + (vy - g * t) * (vy - g * t));
                totalSpeed.Points.Add(new DataPoint(t, formulaForV));
            }
            speedModel.Series.Add(totalSpeed);

            // Координаты X и Y
            var coordinateSeriesX = new LineSeries { Title = "X", Color = OxyColors.Red };
            var coordinateSeriesY = new LineSeries { Title = "Y", Color = OxyColors.Blue };

            for (double t = 0; t <= finalTime; t += 0.1)
            {
                coordinateSeriesX.Points.Add(new DataPoint(t, vx * t));
                double formulaForY = startHeight + vy * t - 0.5 * g * t * t;
                coordinateSeriesY.Points.Add(new DataPoint(t, formulaForY));
            }

            coordModel.Series.Add(coordinateSeriesX);
            coordModel.Series.Add(coordinateSeriesY);

            // Обновление графиков
            trajectoryView.InvalidatePlot(true);
            speedView.InvalidatePlot(true);
            coordView.InvalidatePlot(true);
        }
    }
}