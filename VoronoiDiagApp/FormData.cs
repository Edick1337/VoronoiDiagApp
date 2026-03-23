using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoronoiDiagApp.Properties;

namespace VoronoiDiagApp {
  public partial class FormData : Form {
    public enum eDATA_ERROR {
      NONE,
      LINE_HAS_WRONG_FORMAT,
      LATITUDE_NOT_NUMERIC,
      LONGITUDE_NOT_NUMERIC,
      OCCUPANCY_NOT_NUMERIC
    }

    private const float MAP_LEFT_LONGITUDE = 37.45304f;
    private const float MAP_TOP_LATITUDE = 47.19705f;
    private const float LONGITUDE_TO_MAP_SCALE = 5814.0f / 0.24949f;
    private const float LATITUDE_TO_MAP_SCALE = 5742.0f / 0.16752f;
    private const byte OCCUPANCY_LOWER_BOUND = 60;
    private const byte OCCUPANCY_MID_BOUND = 90;
    private static readonly Color COLOR_LOWER_BOUND = Color.Red;
    private static readonly Color COLOR_MID_BOUND = Color.DarkOrange;
    private static readonly Color COLOR_HIGHER_BOUND = Color.Green;

    public tSCHOOL_COORD[] SchoolCoord;

    public FormData() {
      InitializeComponent();
      LoadCoords();
    }

    private eDATA_ERROR LoadCoords() {
      var s = Resources.Coords;
      s = s.Replace(" ", "");
      s = s.Replace("\r\n", ";");
      var lines = s.Split(';');
      lines = lines.Skip(1).ToArray();  

      SchoolCoord = new tSCHOOL_COORD[lines.Length];

      var dataError = eDATA_ERROR.NONE;
      var lineErrorInd = 0;
      for (var i = 0; i < lines.Length; i++) {
        var words = lines[i].Split('\t');
        if (words.Length != 4) {
          dataError = eDATA_ERROR.LINE_HAS_WRONG_FORMAT;
          break;
        }

        SchoolCoord[i].name = words[0];

        if (double.TryParse(words[1], out var temp) == false) {
          lineErrorInd = i + 1;
          dataError = eDATA_ERROR.LATITUDE_NOT_NUMERIC;
          break;
        }

        SchoolCoord[i].latitude = (float)temp;

        if (double.TryParse(words[2], out temp) == false) {
          lineErrorInd = i + 1;
          dataError = eDATA_ERROR.LONGITUDE_NOT_NUMERIC;
          break;
        }

        SchoolCoord[i].longitude = (float)temp;

        if (double.TryParse(words[3], out temp) == false) {
          lineErrorInd = i + 1;
          dataError = eDATA_ERROR.OCCUPANCY_NOT_NUMERIC;
          break;
        }

        SchoolCoord[i].occupancy = (ushort)Math.Round(temp);

        SchoolCoord[i].color = SchoolCoord[i].occupancy > OCCUPANCY_LOWER_BOUND
          ? SchoolCoord[i].occupancy > OCCUPANCY_MID_BOUND ? COLOR_HIGHER_BOUND : COLOR_MID_BOUND
          : COLOR_LOWER_BOUND;

        dataGridView1.Rows.Add(words);
      }

      switch (dataError) {
        case eDATA_ERROR.LATITUDE_NOT_NUMERIC: {
          MessageBox.Show("Error in school latitude in line (1-based) #" + lineErrorInd);
        }
          break;

        case eDATA_ERROR.LINE_HAS_WRONG_FORMAT: {
          MessageBox.Show(
            "Error in coordinates data - line of text file doesn't correspond scheme: \"school_name\"\\t\"latitude\"\\t\"longitude\"\\t\"occupancy\"\\r\\n");
        }
          break;

        case eDATA_ERROR.LONGITUDE_NOT_NUMERIC: {
          MessageBox.Show("Error in school longitude in line (1-based) #" + lineErrorInd);
        }
          break;
        case eDATA_ERROR.OCCUPANCY_NOT_NUMERIC: {
          MessageBox.Show("Error in school occupancy in line (1-based) #" + lineErrorInd);
        }
          break;
      }

      return dataError;
    }

    public void GetMapCoordBySchoolInd(int ind, out float xMap, out float yMap) {
      xMap = (SchoolCoord[ind].longitude - MAP_LEFT_LONGITUDE) * LONGITUDE_TO_MAP_SCALE;
      yMap = (MAP_TOP_LATITUDE - SchoolCoord[ind].latitude) * LATITUDE_TO_MAP_SCALE;
    }

    public struct tSCHOOL_COORD {
      public string name;
      public float latitude;
      public float longitude;
      public ushort occupancy;
      public Color color;
    }
  }
}