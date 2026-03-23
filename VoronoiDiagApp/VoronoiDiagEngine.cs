using System;
using System.Collections.Generic;
using System.Drawing;

namespace VoronoiDiagApp {
  public class tVoronoi_DIAG_ENGINE {
    private const float MACHINE_ZERO = 1.0e-5f;
    public List<tEDGE> edges;

    public tPOINT[] initialPoints;
    public List<tNODE> nodes;

    private static tNODE GetNodeForThreePoints(tPOINT p1, tPOINT p2, tPOINT p3) {
      tNODE result;
      // Из http://algolist.manual.ru/maths/geom/equation/circle.php
      // коэффициент наклона для линии, проходящей через p1, p2
      var m12 = (p2.y - p1.y) / (p2.x - p1.x + MACHINE_ZERO);

      // Через p2, p3
      var m32 = (p3.y - p2.y) / (p3.x - p2.x + MACHINE_ZERO);

      // Центр окружности (xO, yO), проходящей через p1, p2, p3
      var xO = m12 * m32 * (p1.y - p3.y) + m32 * (p1.x + p2.x) - m12 * (p2.x + p3.x);
      xO /= 2 * (m32 - m12 + MACHINE_ZERO);

      var yO = -1 / (m12 + MACHINE_ZERO);
      yO = yO * (xO - (p1.x + p2.x) / 2) + (p1.y + p2.y) / 2;

      result.x = xO;
      result.y = yO;

      result.ownersList = new List<int>();
      result.ownersList.Add(p1.uInd);
      result.ownersList.Add(p2.uInd);
      result.ownersList.Add(p3.uInd);
      result.edgesInd = new List<int>();
      return result;
    }

    private List<tPOINT_DIST> GetPointsAscendingDistance(float x, float y) {
      var points = new List<tPOINT_DIST>();

      for (var ind = 0; ind < initialPoints.Length; ind++) {
        var dist2 = (float)Math.Pow(x - initialPoints[ind].x, 2) + (float)Math.Pow(y - initialPoints[ind].y, 2);

        int biggerDistInd;
        for (biggerDistInd = 0; biggerDistInd < points.Count; biggerDistInd++)
          if (dist2 < points[biggerDistInd].dist2)
            break;
        tPOINT_DIST pointToInsert;
        pointToInsert.uInd = initialPoints[ind].uInd;
        pointToInsert.dist2 = dist2;
        points.Insert(biggerDistInd,
          pointToInsert);
      }

      return points;
    }

    private List<tNODE> GetListOfNodes() {
      var nodes = new List<tNODE>();
      // Выполнить перебор всех возможных комбинаций точек с координатами школ
      // initialPoints.Length - длина массива координат школ (в штуках, например 10 школ)
      for (var ind = 0; ind < initialPoints.Length; ind++)
      for (var i = ind + 1; i < initialPoints.Length; i++)
      for (var j = i + 1; j < initialPoints.Length; j++) {
        // Для каждой комбинации из трёх точек [ind], [i], [j] (школ) выполняется поиск координат узла, равноудалённого от этих трёх точек
        var node = GetNodeForThreePoints(initialPoints[ind], initialPoints[i], initialPoints[j]);

        // Необходимо проверить, является ли узел "настоящим", т.е. расположены ли все остальные точки, кроме упомянутых трёх, дальше от узла
        // Формирование списка из всех точек (школ), отсортированного по возрастанию расстояния от узла
        var pointsDist = GetPointsAscendingDistance(node.x, node.y);

        /*
         * Каждая точка имеет свой уникальный идентификатор .uInd, по которому можно понять, какой школе она отвечает.
         * В свою очередь, узел имеет перечень его "владельцев", т.е. тех 3х точек, которые его образовали. "Владелец" идентифицируется с помощью uInd значения.
         * Если три "владельца" узла являются первыми тремя точками в сгенерированном сортированном списке pointsDist, то это -- "настоящий" узел и он будет использован в
         * дальнейших расчётах.
         * Пример "ненастоящегого, паразитного" узла: узел, который отвечает школе №5, №64 и №67, которые находятся в разных концах города. Он не должен
         * использоваться далее в расчётах, т.к. есть другие школы, которые будут расположены ближе к узлу, чем его "владельцы"
        */
        if ((pointsDist[0].uInd == node.ownersList[0] || pointsDist[0].uInd == node.ownersList[1] ||
             pointsDist[0].uInd == node.ownersList[2])
            && (pointsDist[1].uInd == node.ownersList[0] || pointsDist[1].uInd == node.ownersList[1] ||
                pointsDist[1].uInd == node.ownersList[2])
            && (pointsDist[2].uInd == node.ownersList[0] || pointsDist[2].uInd == node.ownersList[1] ||
                pointsDist[2].uInd == node.ownersList[2]))
          // initialPoints[ind], [i], [j] -- ближайшие точки к узлу, это "правильный" узел, он должен быть добавлен в список узлов
          nodes.Add(node);
      }

      return nodes;
    }

    private List<tEDGE> GetListOfEdges() {
      var edges = new List<tEDGE>(); // создаем список рёбер

      // Соединение пар узлов с помощью рёбер
      for (var i = 0; i < nodes.Count; i++)
      for (var j = i + 1; j < nodes.Count; j++) {
        /*
         * Для каждой пары узлов выполняется подсчёт количества "владельцев" с одинаковым .uId
         * Возможны варианты, что у 2х узлов:
         *    - не будет общих "владельцев", например один узел в Ильичёвском районе и второй в Приморском. Ребра не будет.
         *    - два общих "владельца" - ребро может быть образовано
         *    - три общих "владельца" - это нештатная ситуация, такого не должно быть, потому что эти узлы по сути совпадают. Игнорируем это, если случится.
        */
        var commonOwnerUidList = new List<int>(); // создаем список общих владельцев
        for (var k = 0; k < nodes[i].ownersList.Count; k++)
        for (var l = 0; l < nodes[j].ownersList.Count; l++)
          if (nodes[i].ownersList[k] == nodes[j].ownersList[l]) // если индекс "владельцев" узла совпадает
            commonOwnerUidList.Add(nodes[i].ownersList[k]); // то добавляем его в список общих индексов

        if (commonOwnerUidList.Count == 2) {
          // Данная пара узлов имеет два общих "владельца". Ребро будет образовано, как, например, ребро, отделяющее ячейки Вороного школ 63 и 64.

          tEDGE edgeToAdd; // Необходимо создать и заполнить структуру с данными ребра...
          edgeToAdd.x1 = nodes[i].x; // сохраняем координаты первого узла (X) для данного ребра
          edgeToAdd.y1 = nodes[i].y; // Y
          edgeToAdd.x2 = nodes[j].x; // сохраняем координаты второго узла (X) для данного ребра
          edgeToAdd.y2 = nodes[j].y; // Y
          // Ребро тоже имеет список своих "владельцев". Ребро между ячейками школ 63 и 64 будет, соотвественно, иметь "владельцев" 63 и 64
          edgeToAdd.ownerList = commonOwnerUidList;
          edges.Add(edgeToAdd);

          // Узлы должны знать, к каким рёбрам они принадлежат, поэтому заполняем список индексов рёбер
          nodes[i].edgesInd.Add(edges.Count - 1);
          nodes[j].edgesInd.Add(edges.Count - 1);
        }
      }

      // Также необходимо обработать рёбра, которые выходят из одного узла и идут в "бесконечность" (за край экрана)
      for (var nodeInd = 0; nodeInd < nodes.Count; nodeInd++)
        // Проверим, какие узлы не имеют 3х рёбер. "Владельцев" 3, и рёбер должно быть 3
        if (nodes[nodeInd].edgesInd.Count == 2) {
          // Узел с двумя рёбрами найден
          // Теперь выполним поиск "владельца" узла, который ещё не имеет своего ребра

          var nodeOwnersListfloatd = new List<int>(); // создаем список владельцев узла

          // Добавляем дважды перечень "владельцев" узла в список nodeOwnersListfloatd
          nodeOwnersListfloatd.AddRange(nodes[nodeInd].ownersList);
          nodeOwnersListfloatd.AddRange(nodes[nodeInd].ownersList);

          /*
           * Просмотр nodeOwnersListfloatd и удаление "владельцев" вузла, которые уже имеют свои ребра. Оставшийся "владелец" будет использован
           * для построения пока что несуществующего ребра
          */
          for (var i = 0; i < nodes[nodeInd].edgesInd.Count; i++) {
            // Для каждого ребра, которое относится к узлу...
            var edgeIndCurrent =
              nodes[nodeInd].edgesInd[i]; // ...определяем индекс ребра в перечне рёбер, принадлежащих узлу...
            // ... и удаляем из списка nodeOwnersListfloatd "владельцев" ребра с заранее определённым индексом edgeIndCurrent
            nodeOwnersListfloatd.Remove(edges[edgeIndCurrent].ownerList[0]);
            nodeOwnersListfloatd.Remove(edges[edgeIndCurrent].ownerList[1]);
          }

          /*
           * Теперь имеем список из двух "владельцев" узлов, которые ещё не имеют ребра между ними.
           * Нужно построить какую-то точку, которая лежит на прямой, проходящей через ребро, и будет использована как второй псевдо-конец ребра, для построения на экране.
           * Мы можем рисовать на экране только отрезка, пусть даже если один его конец будет непонятно где.
          */

          tPOINT middlePoint; // Найдём сначала точку, которая равноудалена от 2х "владельцев" узла
          middlePoint.x = (initialPoints[nodeOwnersListfloatd[0]].x + initialPoints[nodeOwnersListfloatd[1]].x) / 2;
          middlePoint.y = (initialPoints[nodeOwnersListfloatd[0]].y + initialPoints[nodeOwnersListfloatd[1]].y) / 2;

          // Расстояние между найденной средней точкой и текущим узлом по х и у осям:
          var deltaX = middlePoint.x - nodes[nodeInd].x;
          var deltaY = middlePoint.y - nodes[nodeInd].y;

          /*
           * Средняя точка не является текущим узлом, поэтому найдём две равноудалённые и расположенные достаточно далеко точки от двух "владельцев" текущего узла, которые будут вне экрана,
           * просто увеличив расстояния по осям пропорционально.
          */
          var x2Plus = nodes[nodeInd].x + deltaX * 100;
          var y2Plus = nodes[nodeInd].y + deltaY * 100;

          var x2Minus = nodes[nodeInd].x - deltaX * 100;
          var y2Minus = nodes[nodeInd].y - deltaY * 100;

          tEDGE edgeToAdd; // Необходимо создать и заполнить структуру с данными ребра.
          edgeToAdd.ownerList = new List<int>();
          edgeToAdd.ownerList.AddRange(nodeOwnersListfloatd);

          edgeToAdd.x1 = nodes[nodeInd].x;
          edgeToAdd.y1 = nodes[nodeInd].y;

          // Временное назначение, просто чтобы разрешить копирование полей рёбер - ниже будет присвоено правильное значение
          edgeToAdd.x2 = middlePoint.x;
          edgeToAdd.y2 = middlePoint.y;

          // Теперь найдём, какой из вариантов отрезка хороший и идёт куда-то за экран, а какой -- нет и пересекает другие рёбра.
          var edgePlus = edgeToAdd;
          var edgeMinus = edgeToAdd;

          edgePlus.x2 = x2Plus;
          edgePlus.y2 = y2Plus;

          edgeMinus.x2 = x2Minus;
          edgeMinus.y2 = y2Minus;

          float sumOfAbsAnglesPlus = 0;
          float sumOfAbsAnglesMinus = 0;

          /*
           * Рассчитаем сумму углов между потенциальным новым ребром и двумя существующими для текущего узла.
           * Для какого ребра она больше - то и есть хорошее, правильное.
          */
          for (var i = 0; i < 2; i++) {
            sumOfAbsAnglesPlus +=
              Math.Abs(AngleBetweenTwoEdgesWithCommonNode(edges[nodes[nodeInd].edgesInd[i]], edgePlus));
            sumOfAbsAnglesMinus +=
              Math.Abs(AngleBetweenTwoEdgesWithCommonNode(edges[nodes[nodeInd].edgesInd[i]], edgeMinus));
          }

          // То самое обещанное "ниже будет присвоено правильное значение"
          edgeToAdd = sumOfAbsAnglesPlus > sumOfAbsAnglesMinus ? edgePlus : edgeMinus;

          edges.Add(edgeToAdd);
          nodes[nodeInd].edgesInd.Add(edges.Count - 1);
        }

      return edges;
    }

    private static float AngleBetweenTwoEdgesWithCommonNode(tEDGE e1, tEDGE e2) {
      float dx1; // координата X первого ребра
      float dy1; // координата Y первого ребра

      // если координаты нового ребра и существующего равны, то ...
      if (Math.Abs(e1.x1 - e2.x1) < MACHINE_ZERO && Math.Abs(e1.y1 - e2.y1) < MACHINE_ZERO) {
        dx1 = e1.x2 - e1.x1;
        dy1 = e1.y2 - e1.y1;
      }
      else {
        dx1 = e1.x1 - e1.x2;
        dy1 = e1.y1 - e1.y2;
      }

      var dx2 = e2.x2 - e2.x1; // координата X второго ребра
      var dy2 = e2.y2 - e2.y1; // координата Y второго ребра

      var l1 = (float)Math.Pow(dx1 * dx1 + dy1 * dy1, 0.5); // длина первого ребра
      var l2 = (float)Math.Pow(dx2 * dx2 + dy2 * dy2, 0.5); // длина второго ребра

      float cosAlpha = 0;
      if (l1 > MACHINE_ZERO && l2 > MACHINE_ZERO) {
        // если длины рёбер больше 0
        cosAlpha = dx1 * dx2 + dy1 * dy2; // находим скалярное произведение
        cosAlpha /= l1 * l2; // и делим его на длины рёбер, тем самым находим косинус угла между рёбрами
      }

      var alpha = (float)Math.Acos(cosAlpha); // arccos(cos(a)) = a
      return alpha;
    }

    public void CalculateNodes() {
      nodes = GetListOfNodes();
    }

    public void CalculateEdges() {
      edges = GetListOfEdges();
    }

    public struct tPOINT {
      public float x, y;
      public int uInd;
      public Color color;
    }

    private struct tPOINT_DIST {
      public int uInd;
      public float dist2;
    }

    public struct tNODE {
      public List<int> ownersList;
      public float x, y;
      public List<int> edgesInd;
    }

    public struct tEDGE {
      public List<int> ownerList;
      public float x1, y1, x2, y2;
    }
  }
}