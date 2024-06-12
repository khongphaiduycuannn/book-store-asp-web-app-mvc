$(document).ready(function () {
  lineChart();
  donutChart();
  pieChart();
  $(window).resize(function () {
    window.lineChart.redraw();
    window.donutChart.redraw();
    window.pieChart.redraw();
  });
});
function lineChart() {
  var months = [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ];
  window.lineChart = Morris.Line({
    element: "line-chart",
    data: [
      {
        m: "2023-12", // <-- valid timestamp strings
        a: 170,
      },
      {
        m: "2024-01",
        a: 54,
      },
      {
        m: "2024-02",
        a: 54,
      },
      {
        m: "2024-03",
        a: 243,
      },
      {
        m: "2024-04",
        a: 206,
      },
      {
        m: "2024-05",
        a: 161,
      },
      {
        m: "2024-06",
        a: 187,
      },
    ],

    xkey: "m",
    ykeys: ["a"],
    labels: ["Doanh thu"],
    lineColors: ["#0BA462"],
    xLabelFormat: function (x) {
      var month = months[x.getMonth()];
      return month;
    },
    dateFormat: function (x) {
      var month = months[new Date(x).getMonth()];
      return month;
    },
    resize: true,
    redraw: true,
  });
}
function donutChart() {
  window.donutChart = Morris.Donut({
    element: "donut-chart",
    data: [
      { label: "Muong Thanh", value: 50 },
      { label: "Grand", value: 25 },
      { label: "Holiday", value: 5 },
      { label: "Luxury", value: 10 },
    ],
    backgroundColor: "#f2f5fa",
    labelColor: "#009688",
    colors: ["#0BA462", "#39B580", "#67C69D", "#95D7BB"],
    resize: true,
    redraw: true,
  });
}
function pieChart() {
  var paper = Raphael("pie-chart");
  paper.piechart(100, 100, 90, [18.373, 18.686, 2.867, 23.991, 9.592, 0.213], {
    legend: [
      "Windows/Windows Live",
      "Server/Tools",
      "Online Services",
      "Business",
      "Entertainment/Devices",
      "Unallocated/Other",
    ],
  });
}
