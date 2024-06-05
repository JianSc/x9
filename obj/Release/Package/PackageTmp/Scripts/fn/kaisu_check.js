function check(obj, name) {
    if ($(obj).hasClass("rbg")) {
        $(obj).removeClass("rbg");
    } else {
        $(obj).addClass("rbg");
    }

    var dom = $(".tma-box .tb-m");
    var sx = "家禽,野兽,鼠,牛,虎,兔,龙,蛇,马,羊,猴,鸡,狗,猪";
    if (sx.indexOf(name) > -1) {
        $.ajax({
            url: "/handle/javascript_sx",
            type: "POST",
            async: false,
            data: { s: name },
            success: function (data) {
                switch (data.type) {
                    case "suc":
                        $(dom).each(function () {
                            var ball = $(this).find("input[name='ball']").val();
                            if (data.msg.indexOf(ball) > -1) {
                                var xclass = $(this).hasClass("tb-mA");
                                if (!xclass) {
                                    $(this).addClass("tb-mA");
                                } else {
                                    $(this).removeClass("tb-mA");
                                }
                            }
                        })
                        return;
                    case "err":
                        layer.msg(data.msg);
                        return;
                }
            }

        })
        return;
    }
    switch (name) {
        case "单":
            var s;
            for (var i = 1; i < 49; i++) {
                if (i != 1) {
                    s += ",";
                }
                if (i % 2 != 0) {
                    var j = i.toString().length;
                    if (j < 2) {
                        s += '0' + i.toString();
                    } else {
                        s += i.toString();
                    }
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "双":
            var s;
            for (var i = 1; i < 49; i++) {
                if (i != 1) {
                    s += ",";
                }
                if (i % 2 == 0) {
                    var j = i.toString().length;
                    if (j < 2) {
                        s += '0' + i.toString();
                    } else {
                        s += i.toString();
                    }
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "大":
            var s;
            for (var i = 25; i < 49; i++) {
                if (i != 1) {
                    s += ",";
                }
                var j = i.toString().length;
                if (j < 2) {
                    s += '0' + i.toString();
                } else {
                    s += i.toString();
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "小":
            var s;
            for (var i = 1; i < 25; i++) {
                if (i != 1) {
                    s += ",";
                }
                var j = i.toString().length;
                if (j < 2) {
                    s += '0' + i.toString();
                } else {
                    s += i.toString();
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "大单":
            var s;
            for (var i = 25; i < 49; i++) {
                if (i != 1) {
                    s += ",";
                }
                if (i % 2 != 0) {
                    var j = i.toString().length;
                    if (j < 2) {
                        s += '0' + i.toString();
                    } else {
                        s += i.toString();
                    }
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "大双":
            var s;
            for (var i = 25; i < 49; i++) {
                if (i != 1) {
                    s += ",";
                }
                if (i % 2 == 0) {
                    var j = i.toString().length;
                    if (j < 2) {
                        s += '0' + i.toString();
                    } else {
                        s += i.toString();
                    }
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "小单":
            var s;
            for (var i = 1; i < 25; i++) {
                if (i != 1) {
                    s += ",";
                }
                if (i % 2 != 0) {
                    var j = i.toString().length;
                    if (j < 2) {
                        s += '0' + i.toString();
                    } else {
                        s += i.toString();
                    }
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "小双":
            var s;
            for (var i = 1; i < 25; i++) {
                if (i != 1) {
                    s += ",";
                }
                if (i % 2 == 0) {
                    var j = i.toString().length;
                    if (j < 2) {
                        s += '0' + i.toString();
                    } else {
                        s += i.toString();
                    }
                }
            }
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "合单":
            var s = "01,03,05,07,09,10,12,14,16,18,21,23,25,27,29,30,32,34,36,38,41,43,45,47";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "合双":
            var s = "02,04,06,08,11,13,15,17,19,20,22,24,26,28,31,33,35,37,39,40,42,44,46,48";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "合大":
            var s = "07,08,09,16,17,18,19,25,26,27,28,29,34,35,36,37,38,39,43,44,45,46,47,48";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "合小":
            var s = "01,02,03,04,05,06,10,11,12,13,14,15,20,21,22,23,24,30,31,32,33,40,41,42";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "尾大":
            var s = "05,06,07,08,09,15,16,17,18,19,25,26,27,28,29,35,36,37,38,39,45,46,47,48";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "尾小":
            var s = "01,02,03,04,10,11,12,13,14,20,21,22,23,24,30,31,32,33,34,40,41,42,43,44";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "红波":
            var s = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "绿波":
            var s = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "蓝波":
            var s = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "红单":
            var s = "01,07,13,19,23,29,35,45";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "红双":
            var s = "02,08,12,18,24,30,34,40,46";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "红大":
            var s = "29,30,34,35,40,45,46";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "红小":
            var s = "01,02,07,08,12,13,18,19,23,24";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "蓝单":
            var s = "03,09,15,25,31,37,41,47";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "蓝双":
            var s = "04,10,14,20,26,36,42,48";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "蓝大":
            var s = "25,26,31,36,37,41,42,47,48";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "蓝小":
            var s = "03,04,09,10,14,15,20";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "绿单":
            var s = "05,11,17,21,27,33,39,43,49";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "绿双":
            var s = "06,16,22,28,32,38,44";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "绿大":
            var s = "27,28,32,33,38,39,43,44,49";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
        case "绿小":
            var s = "05,06,11,16,17,21,22";
            $(dom).each(function () {
                var ball = $(this).find("input[name='ball']").val();
                if (s.indexOf(ball) > -1) {
                    var xclass = $(this).hasClass("tb-mA");
                    if (!xclass) {
                        $(this).addClass("tb-mA");
                    } else {
                        $(this).removeClass("tb-mA");
                    }
                }
            })
            return;
    }
}