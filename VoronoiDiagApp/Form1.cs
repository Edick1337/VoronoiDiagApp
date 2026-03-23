using System;
using System.Drawing;
using System.Windows.Forms;
using VoronoiDiagApp.Properties;

namespace VoronoiDiagApp {
  public partial class Form1 : Form {
    private const int POINT_SIZE_ZOOM_ENABLED = 14;
    private const int POINT_SIZE_ZOOM_DISABLED = 5;
    private const int NODE_SIZE = 6;
    private const int FONT_SIZE_ZOOM_ENABLED = 16;
    private const int FONT_SIZE_ZOOM_DISABLED = 8;
    private const int LINE_THICKNESS_ZOOM_ENABLED = 2;
    private const int LINE_THICKNESS_ZOOM_DISABLED = 1;
    private readonly int MapHeight;

    private readonly int MapWidth;

    private readonly tVoronoi_DIAG_ENGINE VoronoiEngine = new tVoronoi_DIAG_ENGINE();
    private FormData FormSchoolCoord;
    private RectangleF ZoomedMapRectangle;
    private bool ZoomEnabled;

    public Form1() {
      InitializeComponent();
      pcb_Diagram.Image = Resources.MariupolMap;
      MapWidth = Resources.MariupolMap.Width;
      MapHeight = Resources.MariupolMap.Height;

      FormSchoolCoord = new FormData();

      if (FormSchoolCoord == null || FormSchoolCoord.IsDisposed) FormSchoolCoord = new FormData();

      VoronoiEngine.initialPoints = new tVoronoi_DIAG_ENGINE.tPOINT[FormSchoolCoord.SchoolCoord.Length];
      for (var i = 0; i < VoronoiEngine.initialPoints.Length; i++) {
        VoronoiEngine.initialPoints[i].uInd = i;
        VoronoiEngine.initialPoints[i].color = FormSchoolCoord.SchoolCoord[i].color;
        FormSchoolCoord.GetMapCoordBySchoolInd(i, out VoronoiEngine.initialPoints[i].x,
          out VoronoiEngine.initialPoints[i].y);
      }
    }

    private void btn_Start_Click(object sender, EventArgs e) {
      VoronoiEngine.CalculateNodes();
      VoronoiEngine.CalculateEdges();

      timer1.Enabled = true;
    }

    private void timer1_Tick(object sender, EventArgs e) {
      var sp = new Pen(Color.Black);
      sp.Width = ZoomEnabled ? LINE_THICKNESS_ZOOM_ENABLED : LINE_THICKNESS_ZOOM_DISABLED;

      for (var i = 0; i < VoronoiEngine.initialPoints.Length; i++) {
        var sb = new SolidBrush(VoronoiEngine.initialPoints[i].color);
        int xBox, yBox;
        MapCoordsToBox((int)VoronoiEngine.initialPoints[i].x, (int)VoronoiEngine.initialPoints[i].y, out xBox,
          out yBox);

        int pointSize;

        pointSize = ZoomEnabled ? POINT_SIZE_ZOOM_ENABLED : POINT_SIZE_ZOOM_DISABLED;

        pcb_Diagram.CreateGraphics().FillEllipse(sb, xBox - pointSize / 2,
          yBox - pointSize / 2,
          pointSize, pointSize);

        int fontSize;
        fontSize = ZoomEnabled ? FONT_SIZE_ZOOM_ENABLED : FONT_SIZE_ZOOM_DISABLED;
        var fullName = $"{FormSchoolCoord.SchoolCoord[i].name} ({FormSchoolCoord.SchoolCoord[i].occupancy}%)";
        pcb_Diagram.CreateGraphics().DrawString(fullName, new Font("Arial", fontSize, FontStyle.Bold), sb,
          xBox - pointSize / 2, yBox - pointSize / 2 + 10);
      }

      if (VoronoiEngine.nodes != null)
        for (var i = 0; i < VoronoiEngine.nodes.Count; i++) {
          MapCoordsToBox((int)VoronoiEngine.nodes[i].x, (int)VoronoiEngine.nodes[i].y, out var xBox, out var yBox);

          pcb_Diagram.CreateGraphics().DrawRectangle(sp, xBox - NODE_SIZE / 2,
            yBox - NODE_SIZE / 2,
            NODE_SIZE, NODE_SIZE);
        }

      if (VoronoiEngine.edges != null)
        for (var i = 0; i < VoronoiEngine.edges.Count; i++) {
          MapCoordsToBox((int)VoronoiEngine.edges[i].x1, (int)VoronoiEngine.edges[i].y1, out var xBox1, out var yBox1);
          MapCoordsToBox((int)VoronoiEngine.edges[i].x2, (int)VoronoiEngine.edges[i].y2, out var xBox2, out var yBox2);

          pcb_Diagram.CreateGraphics().DrawLine(sp, xBox1, yBox1, xBox2, yBox2);
        }

      timer1.Enabled = false;
    }

    private void btn_SchoolsData_Click(object sender, EventArgs e) {
      if (FormSchoolCoord == null || FormSchoolCoord.IsDisposed) FormSchoolCoord = new FormData();

      FormSchoolCoord.Show();
    }

    private void MapCoordsToBox(int xMap, int yMap, out int xBox, out int yBox) {
      if (ZoomEnabled) {
        xBox = (int)(xMap - ZoomedMapRectangle.Left);
        yBox = (int)(yMap - ZoomedMapRectangle.Top);
      }
      else {
        var boxWidth = pcb_Diagram.Width;
        var boxHeight = pcb_Diagram.Height;

        int mapWidthInBox;
        int mapHeightInBox;
        int mapXleftInBox;
        int mapYtopInBox;

        if (MapWidth / MapHeight > boxWidth / boxHeight) {
          mapWidthInBox = boxWidth;
          mapHeightInBox = MapHeight * mapWidthInBox / MapWidth;

          mapXleftInBox = 0;
          mapYtopInBox = boxHeight / 2 - mapHeightInBox / 2;
        }
        else {
          mapHeightInBox = boxHeight;
          mapWidthInBox = MapWidth * mapHeightInBox / MapHeight;

          mapXleftInBox = boxWidth / 2 - mapWidthInBox / 2;
          mapYtopInBox = 0;
        }

        var scale = (float)mapHeightInBox / MapHeight;

        xBox = (int)(xMap * scale) + mapXleftInBox;
        yBox = (int)(yMap * scale) + mapYtopInBox;
      }
    }

    private void pcb_Diagram_MouseClick(object sender, MouseEventArgs e) {
      if (e.Button != MouseButtons.Left) return;
      var mapWidth = Resources.MariupolMap.Width;
      var mapHeight = Resources.MariupolMap.Height;

      var boxWidth = pcb_Diagram.Width;
      var boxHeight = pcb_Diagram.Height;

      int mapWidthInBox;
      int mapHeightInBox;
      int mapXleftInBox;
      int mapYtopInBox;

      if (mapWidth / mapHeight > boxWidth / boxHeight) {
        mapWidthInBox = boxWidth;
        mapHeightInBox = mapHeight * mapWidthInBox / mapWidth;

        mapXleftInBox = 0;
        mapYtopInBox = boxHeight / 2 - mapHeightInBox / 2;
      }
      else {
        mapHeightInBox = boxHeight;
        mapWidthInBox = mapWidth * mapHeightInBox / mapHeight;

        mapXleftInBox = boxWidth / 2 - mapWidthInBox / 2;
        mapYtopInBox = 0;
      }

      var xRelativeClick = (e.X - mapXleftInBox) / (float)mapWidthInBox;
      var yRelativeClick = (e.Y - mapYtopInBox) / (float)mapHeightInBox;

      if ((!(xRelativeClick > 0.0) || !(xRelativeClick < 1.0) || !(yRelativeClick > 0.0) || !(yRelativeClick < 1.0))
          && !ZoomEnabled) return;
      if (ZoomEnabled == false) {
        ZoomEnabled = true;

        var sourceRectangleXleft = (int)(xRelativeClick * mapWidth) - boxWidth / 2;
        var sourceRectangleYtop = (int)(yRelativeClick * mapHeight) - boxHeight / 2;
        var sourceRectangleXright = (int)(xRelativeClick * mapWidth) + boxWidth / 2;
        var sourceRectangleYbottom = (int)(yRelativeClick * mapHeight) + boxHeight / 2;

        if (sourceRectangleXleft < 0) {
          sourceRectangleXleft = 0;
          sourceRectangleXright = boxWidth;
        }

        if (sourceRectangleYtop < 0) {
          sourceRectangleYtop = 0;
          sourceRectangleYbottom = boxHeight;
        }

        if (sourceRectangleXright > mapWidth - 1) {
          sourceRectangleXright = mapWidth - 1;
          sourceRectangleXleft = mapWidth - 1 - boxWidth;
        }

        if (sourceRectangleYbottom > mapHeight - 1) {
          sourceRectangleYbottom = mapHeight - 1;
          sourceRectangleYtop = mapHeight - 1 - boxHeight;
        }

        var sourceRectangle = new RectangleF(sourceRectangleXleft, sourceRectangleYtop,
          sourceRectangleXright - sourceRectangleXleft + 1,
          sourceRectangleYbottom - sourceRectangleYtop + 1);
        var destinationRectangle = new RectangleF(0, 0,
          boxWidth,
          boxHeight);

        pcb_Diagram.CreateGraphics().DrawImage(Resources.MariupolMap, destinationRectangle, sourceRectangle,
          GraphicsUnit.Pixel);
        timer1.Enabled = true;
        ZoomedMapRectangle = sourceRectangle;
      }
      else {
        ZoomEnabled = false;
        pcb_Diagram.Image = Resources.MariupolMap;
        timer1.Enabled = true;
      }
    }

    private void Form1_Resize(object sender, EventArgs e) {
      ZoomEnabled = false;
      timer1.Enabled = WindowState != FormWindowState.Minimized;
    }
  }
}