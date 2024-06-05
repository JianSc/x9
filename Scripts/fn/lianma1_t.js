$(function () {
    $("#form1 .tb-m").click(function () {
        var ul = $("#form1").find(".tb-mAA");
        if (!$(this).hasClass("tb-mA") && !$(this).hasClass("tb-mAA") && $("#form1 .tb-mA").length < 10) {
            if (ul.length < 1) {
                $(this).addClass("tb-mAA");
            } else {
                $(this).addClass("tb-mA");
            }
        } else {
            if ($(this).hasClass("tb-mA")) {
                $(this).removeClass("tb-mA");
            } else if ($(this).hasClass("tb-mAA")) {
                var c = $("#form1 .tb-mA");
                if (c.length < 1) {
                    $(this).removeClass("tb-mAA");
                }
            } else {
                layer.alert("最多只能选择10颗球号！");
                return;
            }
        }
    })
    $("#form1 #submit").click(function () {
        var t = $("#form2");
        $(t).html('');
        var script_id = $("#form1 input[name='script_id']").val();
        var dom = $("#form1").find(".tb-mAA");
        var dom2 = $("#form1").find(".tb-mA");
        var money = $("#form1 input[name='moneys']").val();
        if (script_id == '0' || script_id == '') {
            return;
        }
        if (money == '' || money == '0') {
            layer.alert("请输入下注金额！");
            return;
        }
        money = parseInt(money);
        if (isNaN(money)) {
            layer.alert("下注金额只能输入数字！");
            return;
        }
        if (dom2.length < 2) {
            layer.alert("请选择2-10颗球号！");
            return;
        }
        var index = layer.load(1, { shade: [0.3] });
        for (var i = 0; i < dom.length; i++) {
            for (var j = 0; j < dom2.length; j++) {
                var a = $(dom[i]).find("input[name='ball']").val();
                var b = $(dom2[j]).find("input[name='ball']").val();
                var c = $(dom2[j]).find("input[name='beilu']").val();
                var newball = $("<input type=\"hidden\" name=\"ball\" value=" + a + "," + b + " />");
                var newbeilu = $("<input type=\"hidden\" name=\"beilu\" value=" + c + " />");
                var newmoney = $("<input type=\"hidden\" name=\"money\" value=" + money + " />");
                $(t).append($(newball)).append($(newbeilu)).append($(newmoney));
            }
        }

        t.ajaxSubmit({
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
                $(t).html('');
            }
        })
    })
    $("#form1 #reset").click(function () {
        var newform = $("#form2");
        $("#form1 .tb-mA").each(function () {
            $(this).removeClass("tb-mA");
        })
        $("#form1 .tb-mAA").each(function () {
            $(this).removeClass("tb-mAA");
        })
        $(newform).html('');
    })
})