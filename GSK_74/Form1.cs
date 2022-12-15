using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GSK_74
{
    public partial class Form1 : Form
    {
        private readonly Graphics _graphics;

        /// <summary>
        ///  Буффер
        /// </summary>
        private readonly Bitmap _bitmap;

        /// <summary>
        ///  Множество для ТМО
        /// </summary>
        private int[] _setQ = new int[2];

        /// <summary>
        ///  Массив для совместной обработки исходных границ сегмента
        /// </summary>
        private M[] _arrayM;

        /// <summary>
        ///  Цвет закрашивания фигуры
        /// </summary>
        private readonly Pen _drawPen = new Pen(Color.Black, 1);

        /// <summary>
        ///  Выбор операции
        /// </summary>
        private int _operation;

        /// <summary>
        /// Тип фигуры
        /// </summary>
        private char _figure;

        /// <summary>
        /// Проверка на рисование выбранной фигуры
        /// </summary>
        private bool _isPaintFigure;

        private readonly List<MyPoint> _points;

        /// <summary>
        /// Список фигур
        /// </summary>
        private readonly List<List<MyPoint>> _figures;

        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _points = new List<MyPoint>();
            _figures = new List<List<MyPoint>>();
            MouseWheel += Geometric;
        }

        // Обработчик события "Нажатие кнопки"
        private void PictureMouseDown(object sender, MouseEventArgs e)
        {
            _operation = comboBoxGeometric.SelectedIndex;
            if (_operation == 0 && _isPaintFigure)
            {
                switch (_figure)
                {
                    case 's':
                        if (_points.Count < 3)
                        {
                            AddPoint(e);
                            return;
                        }

                        AddPoint(e);
                        CreateCubeSpline();
                        break;
                    case 'p':
                        CreateRegularPolygon(e);
                        _points.Clear();
                        break;
                    default:
                        CreateStrelka1(e);
                        break;
                }

                Fill(_figures[_figures.Count - 1]);
                _isPaintFigure = false;
            }
            // Добавление точки
            else if (MouseButtons.Left == e.Button)
                AddPoint(e);
            // Рисование и добавление фигуры в общей список фигур
            else
            {
                Fill(_points);
                _figures.Add(_points.ToList());
                _points.Clear();
            }

            pictureBox1.Image = _bitmap;
        }

        // Поиск мин/макс Y
        private float[] SearchYMinAndMax(List<MyPoint> pointFs)
        {
            if (pointFs.Count == 0)
                return new float[] {0, 0, 0};

            var min = pointFs[0].Y;
            var max = pointFs[0].Y;
            foreach (var t in pointFs)
            {
                min = t.Y < min ? t.Y : min;
                max = t.Y < max ? max : t.Y;
            }

            min = min < 0 ? 0 : min;
            max = max > pictureBox1.Height ? pictureBox1.Height : max;
            return new[] {min, max};
        }

        #region Создание фигур предлагаемых пользователю

        private void CreateCubeSpline()
        {
            var function = new List<MyPoint>();
            var l = new PointF[4];
            var pv1 = _points[0].ToPoint();
            var pv2 = _points[0].ToPoint();

            const double dt = 0.04;
            double t = 0;
            double xt, yt;
            Point ppred = _points[0].ToPoint(), pt = _points[0].ToPoint();

            pv1.X = (int) (4 * (_points[1].X - _points[0].X));
            pv1.Y = (int) (4 * (_points[1].Y - _points[0].Y));
            pv2.X = (int) (4 * (_points[3].X - _points[2].X));
            pv2.Y = (int) (4 * (_points[3].Y - _points[2].Y));

            l[0].X = 2 * _points[0].X - 2 * _points[2].X + pv1.X + pv2.X; // Ax
            l[0].Y = 2 * _points[0].Y - 2 * _points[2].Y + pv1.Y + pv2.Y; // Ay
            l[1].X = -3 * _points[0].X + 3 * _points[2].X - 2 * pv1.X - pv2.X; // Bx
            l[1].Y = -3 * _points[0].Y + 3 * _points[2].Y - 2 * pv1.Y - pv2.Y; // By
            l[2].X = pv1.X; // Cx
            l[2].Y = pv1.Y; // Cy
            l[3].X = _points[0].X; // Dx 
            l[3].Y = _points[0].Y;
            function.Add(new MyPoint(ppred.X, ppred.Y) {Function = true});
            while (t < 1 + dt / 2)
            {
                xt = ((l[0].X * t + l[1].X) * t + l[2].X) * t + l[3].X;
                yt = ((l[0].Y * t + l[1].Y) * t + l[2].Y) * t + l[3].Y;

                pt.X = (int) Math.Round(xt);
                pt.Y = (int) Math.Round(yt);

                _graphics.DrawLine(_drawPen, ppred, pt);
                ppred = pt;
                function.Add(new MyPoint(ppred.X, ppred.Y));
                t += dt;
            }

            _figures.Add(function);
        }

        private void CreateRegularPolygon(MouseEventArgs e)
        {
            var count = comboBoxVertex.SelectedIndex + 3;
            var points = new List<MyPoint>();
            const int length = 30;
            var r = length / (2 * Math.Sin(Math.PI / count));
            for (var angle = 0.0; angle <= 2 * Math.PI; angle += 2 * Math.PI / count)
            {
                var x = (int) (r * Math.Cos(angle));
                var y = (int) (r * Math.Sin(angle));
                points.Add(new MyPoint((float) (r + x), (float) (r + y)));
            }

            _figures.Add(points);
            ToAndFromCenter(false, new MyPoint(e.X - length, e.Y - length), _figures[_figures.Count - 1]);
        }

        private void CreateStrelka1(MouseEventArgs e)
        {
            var points = new List<MyPoint>
            {
                new MyPoint(e.X - 100, e.Y - 20),
                new MyPoint(e.X + 10, e.Y - 20),
                new MyPoint(e.X + 10, e.Y - 50),
                new MyPoint(e.X + 100, e.Y),
                new MyPoint(e.X + 10, e.Y + 50),
                new MyPoint(e.X + 10, e.Y + 20),
                new MyPoint(e.X - 100, e.Y + 20)
            };
            _figures.Add(points);
        }

        #endregion

        private void AddPoint(MouseEventArgs e)
        {
            _points.Add(new MyPoint(e.X, e.Y));
            if (_points.Count > 1)
                _graphics.DrawLine(_drawPen, _points[_points.Count - 2].ToPoint(),
                    _points[_points.Count - 1].ToPoint());
            pictureBox1.Image = _bitmap;
        }

        private void Fill(List<MyPoint> pointFs)
        {
            if (pointFs[0].Function)
            {
                PaintingLineInFigure(pointFs);
                return;
            }

            var arr = SearchYMinAndMax(pointFs);
            var min = arr[0];
            var max = arr[1];
            var xs = new List<float>();

            for (var y = (int) min; y < max; y++)
            {
                var k = 0;
                for (var i = 0; i < pointFs.Count - 1; i++)
                {
                    k = i < pointFs.Count ? i + 1 : 1;
                    xs = CheckIntersection(xs, i, k, y, pointFs);
                }

                xs = CheckIntersection(xs, k, 0, y, pointFs);
                xs.Sort();

                for (var i = 0; i + 1 < xs.Count; i += 2)
                    _graphics.DrawLine(_drawPen, new Point((int) xs[i], y), new Point((int) xs[i + 1], y));

                xs.Clear();
            }

            pictureBox1.Image = _bitmap;
        }

        // Проверка пересичения прямой Y c отрезком
        private static List<float> CheckIntersection(List<float> xs, int i, int k, int y, List<MyPoint> pointFs)
        {
            if (Check(i, k, y, pointFs))
            {
                var x = -((y * (pointFs[i].X - pointFs[k].X)) - pointFs[i].X * pointFs[k].Y +
                          pointFs[k].X * pointFs[i].Y)
                        / (pointFs[k].Y - pointFs[i].Y);
                xs.Add(x);
            }

            return xs;
        }

        // Условие пересечения
        private static bool Check(int i, int k, int y, List<MyPoint> pointFs) =>
            (pointFs[i].Y < y && pointFs[k].Y >= y) || (pointFs[i].Y >= y && pointFs[k].Y < y);

        #region ТМО

        // Алгоритм теоретико-множественных операций
        private void Tmo()
        {
            var figure1 = _figures[0];
            figure1[0].DoTmo = true;
            var figure2 = _figures[1];
            figure2[0].DoTmo = true;
            var arr = SearchYMinAndMax(figure1);
            var arr2 = SearchYMinAndMax(figure2);
            var minY = arr[0] < arr2[0] ? arr[0] : arr2[0];
            var maxY = arr[1] > arr2[1] ? arr[1] : arr2[1];
            for (var y = (int) minY; y < maxY; y++)
            {
                var a = CalculationListXrAndXl(y, figure1);
                List<float> xAl = a[0];
                List<float> xAr = a[1];
                var b = CalculationListXrAndXl(y, figure2);
                List<float> xBl = b[0];
                List<float> xBr = b[1];
                if (xAl.Count == 0 && xBl.Count == 0)
                    continue;

                _arrayM = new M[xAl.Count * 2 + xBl.Count * 2];
                for (var i = 0; i < xAl.Count; i++)
                    _arrayM[i] = new M(xAl[i], 2);

                var nM = xAl.Count;
                for (var i = 0; i < xAr.Count; i++)
                    _arrayM[nM + i] = new M(xAr[i], -2);

                nM += xAr.Count;
                for (var i = 0; i < xBl.Count; i++)
                    _arrayM[nM + i] = new M(xBl[i], 1);

                nM += xBl.Count;
                for (var i = 0; i < xBr.Count; i++)
                    _arrayM[nM + i] = new M(xBr[i], -1);
                nM += xBr.Count;

                // Сортировка
                for (var write = 0; write < _arrayM.Length; write++)
                for (var sort = 0; sort < _arrayM.Length - 1; sort++)
                    if (_arrayM[sort].X > _arrayM[sort + 1].X)
                    {
                        var buuf = new M(_arrayM[sort + 1].X, _arrayM[sort + 1].Dq);
                        _arrayM[sort + 1] = _arrayM[sort];
                        _arrayM[sort] = buuf;
                    }

                var q = 0;
                List<int> xrl = new List<int>();
                List<int> xrr = new List<int>();
                // Особый случай для правой границы сегмента
                if (_arrayM[0].X >= 0 && _arrayM[0].Dq < 0)
                {
                    xrl.Add(0);
                    q = -_arrayM[1].Dq;
                }

                for (var i = 0; i < nM; i++)
                {
                    var x = _arrayM[i].X;
                    var qnew = q + _arrayM[i].Dq;
                    if (!IncludeQInSetQ(q) && IncludeQInSetQ(qnew))
                        xrl.Add((int) x);
                    else if (IncludeQInSetQ(q) && !IncludeQInSetQ(qnew))
                        xrr.Add((int) x);

                    q = qnew;
                }

                // Если не найдена правая граница последнего сегмента
                if (IncludeQInSetQ(q))
                    xrr.Add(pictureBox1.Height);

                for (var i = 0; i < xrr.Count; i++)
                    _graphics.DrawLine(_drawPen, new Point(xrr[i], y), new Point(xrl[i], y));
            }
        }

        // Нахождение точек пересечения фигуры с прямой Y
        private static List<float>[] CalculationListXrAndXl(int y, List<MyPoint> pointFs)
        {
            var k = 0;
            var xR = new List<float>();
            var xL = new List<float>();
            for (var i = 0; i < pointFs.Count - 1; i++)
            {
                k = i < pointFs.Count ? i + 1 : 1;
                if (Check(i, k, y, pointFs))
                {
                    var x = -((y * (pointFs[i].X - pointFs[k].X))
                                - pointFs[i].X * pointFs[k].Y + pointFs[k].X * pointFs[i].Y)
                            / (pointFs[k].Y - pointFs[i].Y);
                    if (pointFs[k].Y - pointFs[i].Y > 0)
                        xR.Add(x);
                    else
                        xL.Add(x);
                }
            }

            if (Check(k, 0, y, pointFs))
            {
                var x = -((y * (pointFs[k].X - pointFs[0].X))
                            - pointFs[k].X * pointFs[0].Y + pointFs[0].X * pointFs[k].Y)
                        / (pointFs[0].Y - pointFs[k].Y);
                if (pointFs[0].Y - pointFs[k].Y > 0)
                    xR.Add(x);
                else
                    xL.Add(x);
            }

            return new[] {xL, xR};
        }

        // Проверка вхождения Q в множество setQ
        private bool IncludeQInSetQ(int q) => _setQ[0] <= q && q <= _setQ[1];

        #endregion

        #region Геометрические преобразования

        private int _updateAlpha;

        /// <summary>
        /// Поворот фигуры
        /// </summary>
        private void Rotation(int mouse, MouseEventArgs em, List<MyPoint> pointFs)
        {
            float alpha = 0;
            if (mouse > 0)
            {
                alpha += 0.0175f;
                _updateAlpha++;
            }
            else
            {
                alpha -= 0.0175f;
                _updateAlpha--;
            }

            textBox1.Text = _updateAlpha.ToString();
            var e = new MyPoint(em.X, em.Y);
            ToAndFromCenter(true, e, pointFs);

            float[,] matrixRotation =
            {
                {(float) Math.Cos(alpha), (float) Math.Sin(alpha), 0.0f},
                {-(float) Math.Sin(alpha), (float) Math.Cos(alpha), 0.0f},
                {0.0f, 0.0f, 1.0f}
            };
            for (var i = 0; i < pointFs.Count; i++)
                pointFs[i] = Matrix_1x3_x_3x3(pointFs[i], matrixRotation);

            ToAndFromCenter(false, e, pointFs);
        }

        private static void Calculation(float[,] matrix, List<MyPoint> points)
        {
            for (var i = 0; i < points.Count; i++)
                points[i] = Matrix_1x3_x_3x3(points[i], matrix);
        }

        /// <summary>
        ///  Отражение
        /// </summary>
        private void Mirror(List<MyPoint> points)
        {
            // Проверку на 2 точки
            var m = new[,]
            {
                {1, 0, 0},
                {0, 1, 0},
                {-_points[0].X, -_points[0].Y, 1}
            };
            Calculation(m, points);

            var a = _points[0];
            var b = _points[1];
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            var d = Math.Sqrt(dx * dx + dy * dy);
            var cos = (float) (dx / d);
            var sin = (float) (dy / d);

            var r = new[,]
            {
                {cos, -sin, 0},
                {sin, cos, 0},
                {0, 0, 1}
            };
            Calculation(r, points);
            var s = new float[,]
            {
                {1, 0, 0},
                {0, -1, 0},
                {0, 0, 1}
            };
            Calculation(s, points);

            var r1 = new[,]
            {
                {cos, sin, 0},
                {-sin, cos, 0},
                {0, 0, 1}
            };
            Calculation(r1, points);

            var m1 = new[,]
            {
                {1, 0, 0},
                {0, 1, 0},
                {_points[0].X, _points[0].Y, 1}
            };
            Calculation(m1, points);
        }

        /// <summary>
        /// Масштабирование
        /// </summary>
        /// <param name="zoom">коэфициет увеличения</param>
        /// <param name="em">центр с координатами</param>
        /// <param name="points">Фигура</param>
        private static void Zoom(float[] zoom, MouseEventArgs em, List<MyPoint> points)
        {
            if (zoom[0] <= 0) zoom[0] = -0.1f;
            if (zoom[1] <= 0) zoom[1] = -0.1f;
            if (zoom[0] >= 0) zoom[0] = 0.1f;
            if (zoom[1] >= 0) zoom[1] = 0.1f;

            var sx = 1 + zoom[0];
            const int sy = 1;
            float[,] matrix =
            {
                {sx, 0, 0},
                {0, sy, 0},
                {0, 0, 1}
            };

            var e = new MyPoint(em.X, em.Y);
            ToAndFromCenter(true, e, points);

            for (var i = 0; i < points.Count; i++)
                points[i] = Matrix_1x3_x_3x3(points[i], matrix);

            ToAndFromCenter(false, e, points);
        }

        private static void ToAndFromCenter(bool start, MyPoint e, List<MyPoint> pointFs)
        {
            if (start)
            {
                float[,] toCenter =
                {
                    {1, 0, 0},
                    {0, 1, 0},
                    {-e.X, -e.Y, 1}
                };
                for (var i = 0; i < pointFs.Count; i++)
                    pointFs[i] = Matrix_1x3_x_3x3(pointFs[i], toCenter);
            }
            else
            {
                float[,] fromCenter =
                {
                    {1, 0, 0},
                    {0, 1, 0},
                    {e.X, e.Y, 1}
                };
                for (var i = 0; i < pointFs.Count; i++)
                    pointFs[i] = Matrix_1x3_x_3x3(pointFs[i], fromCenter);
            }
        }

        private static MyPoint Matrix_1x3_x_3x3(MyPoint point, float[,] matrix3X3) => new MyPoint
        {
            X = point.X * matrix3X3[0, 0] + point.Y * matrix3X3[1, 0] + point.Third * matrix3X3[2, 0],
            Y = point.X * matrix3X3[0, 1] + point.Y * matrix3X3[1, 1] + point.Third * matrix3X3[2, 1],
            Third = point.X * matrix3X3[0, 2] + point.Y * matrix3X3[1, 2] + point.Third * matrix3X3[2, 2],
            Function = point.Function
        };

        #endregion

        #region Выбор пользователя

        // Начало геометрических преобразований
        private void Geometric(object sender, MouseEventArgs e)
        {
            var figureBuff = _figures[_figures.Count - 1];
            if (figureBuff[0].DoTmo)
            {
                TransformationGeometric(e, figureBuff);
                TransformationGeometric(e, _figures[_figures.Count - 2]);
                _graphics.Clear(Color.White);
                Tmo();
                pictureBox1.Image = _bitmap;
            }
            else
                TransformationGeometric(e, figureBuff);
        }

        private void TransformationGeometric(MouseEventArgs e, List<MyPoint> buff)
        {
            _operation = comboBoxGeometric.SelectedIndex;
            switch (_operation)
            {
                case 1:
                    Rotation(e.Delta, e, buff);
                    break;
                case 2:
                    Zoom(new float[] {e.Delta, e.Delta}, e, buff);
                    break;
                case 3:
                    Mirror(buff);
                    break;
            }

            _graphics.Clear(pictureBox1.BackColor);
            Fill(buff);
        }

        private void PaintingLineInFigure(List<MyPoint> points)
        {
            for (var i = 0; i < points.Count - 1; i++)
                _graphics.DrawLine(_drawPen, points[i].ToPoint(), points[i + 1].ToPoint());
            pictureBox1.Image = _bitmap;
        }

        // Выбор цвета
        private void comboBoxColor_(object sender, EventArgs e)
        {
            switch (comboBoxColor.SelectedIndex)
            {
                case 0:
                    _drawPen.Color = Color.Black;
                    break;
                case 1:
                    _drawPen.Color = Color.Red;
                    break;
                case 2:
                    _drawPen.Color = Color.Green;
                    break;
                case 3:
                    _drawPen.Color = Color.Blue;
                    break;
            }
        }

        // Выбор фигуры для рисования
        private void comboBoxFigures_(object sender, EventArgs e)
        {
            switch (comboBoxFigures.SelectedIndex)
            {
                case 0:
                    _figure = 's';
                    break;
                case 1:
                    _figure = 'p';
                    break;
                default:
                    _figure = 'c';
                    break;
            }

            _isPaintFigure = true;
        }

        // Выбор ТМО
        private void ComboBoxTmo(object sender, EventArgs e)
        {
            switch (comboBoxTMO.SelectedIndex)
            {
                case 0:
                    // объединение
                    _setQ = new[] {1, 3};
                    break;
                case 1:
                    // симметричная разность
                    _setQ = new[] {1, 2};
                    break;
            }
        }

        // Кнопка очистки
        private void buttonClear_(object sender, EventArgs e)
        {
            _points.Clear();
            _figures.Clear();
            _graphics.Clear(Color.White);
            pictureBox1.Image = _bitmap;
        }

        private void ButtonTmo(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            Tmo();
            pictureBox1.Image = _bitmap;
        }

        #endregion

        private struct M
        {
            public float X { get; }
            public int Dq { get; }

            public M(float x, int dQ)
            {
                X = x;
                Dq = dQ;
            }
        }

        public class MyPoint
        {
            public float X;
            public float Y;
            public float Third;
            public bool Function;
            public bool DoTmo;

            public MyPoint(float x = 0.0f, float y = 0.0f, float third = 1.0f)
            {
                X = x;
                Y = y;
                Third = third;
                Function = false;
                DoTmo = false;
            }

            public Point ToPoint() => new Point((int) X, (int) Y);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxGeometric_SelectedIndexChanged(object sender, EventArgs e) =>
            _operation = comboBoxGeometric.SelectedIndex;
    }
}