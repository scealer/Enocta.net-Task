function AylikRapor() {

    $.post("GetMountlyReport",
        function (datax, status) {
            var mo = [];

            $.each(datax[0], function (i, l) {
                mo.push(datax[0][i]);
            });

            var dataViews = {
                labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağutos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                series: [
                    mo
                ]
            };

            var optionsViews = {

                seriesBarDistance: 5,
                classNames: {
                    bar: 'ct-bar ct-azure'
                },

                axisX: {
                    showGrid: true,
                    showLabel: true
                }
            };

            Chartist.Line('#chartViews', dataViews, optionsViews);

        });
}
AylikRapor();




/*Kullanılmıyor */
function AylikSatis() {
    $.post("GetMountlyReport",
        function (datax, status) {
            var m = [];

            $.each(datax[0], function (i, l) {
                m.push(datax[0][i]);
            });

            var dataPerformance = {
                labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağutos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                series: [m]
            };

            var optionsPerformance = {
                showPoint: false,
                lineSmooth: true,
                height: '200px',
                axisX: {
                    showGrid: false,
                    showLabel: true
                },
                axisY: {
                    offset: 40,
                },
                low: 0,
                high: 16,
                height: '250px'
            };

            Chartist.Line('#chart1', dataPerformance, optionsPerformance);

        });
}

//AylikSatis();







/*Kullanılmıyor */
function TarihRapor() {

    $.post("GetDateReport", {
        LastOrderDate: $("#LastOrderDate").val(),
        FirstOrderDate: $("#FirstOrderDate").val()
    },
        function (datax, status) {
            var dataViews = {
                labels: ['Toplam'],
                series: [
                    [datax]
                ]
            };

            var optionsViews = {
                seriesBarDistance: 10,
                classNames: {
                    bar: 'ct-bar ct-azure'
                },
                axisX: {
                    showGrid: false
                }

            };
            var responsiveOptionsViews = [
                ['screen and (max-width: 640px)', {
                    seriesBarDistance: 5,
                    axisX: {
                        labelInterpolationFnc: function (value) {
                            return value[0];
                        }
                    }
                }]

            ];
            Chartist.Bar('#chartViews', dataViews, optionsViews, responsiveOptionsViews);
            Bildirim(datax + "TL geliriniz var.");
        });
}

