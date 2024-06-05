$(function () {
    $(".tma-box .tb-m").click(function () {
        var f = $("#form1");
        var ul = $(f).find(".tb-mA");
        var check = $(this).hasClass("tb-mA");
        if (check) {
            $(this).removeClass("tb-mA");
        } else {
            if (ul.length >= 10) {
                layer.alert("最多只能选择10颗球号！", { title: '提示' });
                return;
            } else {
                $(this).addClass("tb-mA");
            }

        }
    })

    $("#form1 #submit").click(function () {
        var newform = $("#form2");
        $(newform).html('');
        var moneys = $("#form1 input[name='moneys']");
        var script_id = $("#form1 input[name='script_id']").val();
        if (script_id == null || script_id == '0') {
            return;
        }
        if (moneys.val() == '' || moneys.val() == '0') {
            layer.alert("请输入下注金额！", { title: "错误" });
            return;
        }
        var m = parseInt(moneys.val());
        if (isNaN(m)) {
            layer.alert("下注金额只能输入数字！");
            return;
        }
        if ($("#form1 .tb-mA").length < 3) {
            layer.alert("请选择3-10颗球号！");
            return;
        }

        var code;
        var i = 1;
        var xbeilu;

        $("#form1 .tb-mA").each(function () {
            var t = $(this).find("input[name='ball']").val();
            var b = $(this).find("input[name='beilu']").val();
            if (i > 1) {
                code += ",";
                xbeilu += ",";
                code += t;
                xbeilu += b;
            } else {
                code = t;
                xbeilu = b;
            }
            i++;
        })
        var codearr = code.split(',');
        var xbeilu = xbeilu.split(',');
        var index = layer.load(1, { shade: [0.3] });
        for (var i = 0; i < codearr.length - 2; i++) {
            for (var j = i + 1; j < codearr.length; j++) {
                for (var k = j + 1; k < codearr.length; k++) {
                    var ball = $("<input name=\"ball\" type=\"hidden\" value=" + codearr[i] + ',' + codearr[j] + "," + codearr[k] + " />");
                    var beilu = $("<input name=\"beilu\" type=\"hidden\" value=" + xbeilu[j] + " />");
                    var money = $("<input name=\"money\" type=\"hidden\" value=" + moneys.val() + " />");
                    $(newform).append($(ball)).append($(beilu)).append($(money));
                }

            }
        }

        newform.ajaxSubmit({
            url: "/handle/xiazu_view2/" + script_id,
            type: "POST",
            success: function (data) {
                layer.close(index);
                layer.open({
                    type: 1,
                    title: "<i class='fa fa-sign-in'></i> 下注池",
                    resize: false,
                    area: ["600px", "400px"],
                    content: data
                })
                $(newform).html('');
            }
        })
    })

    $("#form1 #reset").click(function () {
        $("#form2").html('');
        $("#form1 .tb-mA").each(function () {
            $(this).removeClass("tb-mA");
        })
    })
})