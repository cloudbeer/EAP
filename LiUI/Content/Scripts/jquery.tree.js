$.fn.extend({
    liTree: function (title, actionurl) {
        var treeHTML = '';
        if (actionurl) {
            var tree = $(this);
            tree.addClass("tree");

            $.get(actionurl, function (xml) {
                var xmlDoc = $(xml);
                treeHTML += '<ul>';
                treeHTML += '	<li><i class="icon i_treeclose"></i><span>' + title + '</span>';
                treeHTML += '		<ul>';

                xmlDoc.find("tree>treeItem").each(function () {
                    if ($(this).find(">treeItem").length > 1)
                        treeHTML += '<li rel="' + $(this).attr("id") + '"><i class="icon i_treeclose"></i><span>' + $(this).attr("value") + '</span>';
                    else
                        treeHTML += '<li rel="' + $(this).attr("id") + '"><i class="icon i_none"></i><span>' + $(this).attr("value") + '</span>';
                });

                treeHTML += '		</ul>';
                treeHTML += '	</li>';
                treeHTML += '</ul>';

                tree.append(treeHTML);
            });

            $(this).find("ul li").live("click", function () {
                if ($(this).find("i:first").attr("class").indexOf("i_treeclose") > 0) {
                    $(this).find(">ul").show('fast');
                    $(this).find("i:first").removeClass("i_treeclose").addClass("i_treeopen");

                    var thisLi = $(this);
                    var rel = thisLi.attr("rel");
                    if (rel > 0) {
                        var innerTreeHTML = "";
                        $.get(actionurl, { id: rel }, function (xml) {
                            var xmlDoc = $(xml);
                            innerTreeHTML += "<ul>";

                            xmlDoc.find("tree>treeItem").each(function () {
                                if ($(this).find(">treeItem").length > 1) {
                                    innerTreeHTML += '<li rel="' + $(this).attr("id") + '">';
                                    innerTreeHTML += '<i class="icon i_treeclose"></i>';
                                    innerTreeHTML += '<span>' + $(this).attr("value") + '</span></li>';
                                }
                                else {
                                    innerTreeHTML += '<li rel="' + $(this).attr("id") + '">';
                                    innerTreeHTML += '<i class="icon i_none"></i>';
                                    innerTreeHTML += '<span>' + $(this).attr("value") + '</span></li>';
                                }
                            });

                            innerTreeHTML += "</ul>";
                            thisLi.find(">ul").remove();
                            thisLi.append(innerTreeHTML);
                            thisLi.find(">ul").show('fast');
                        });
                    }
                    return false;
                }
                else if ($(this).find("i:first").attr("class").indexOf("i_treeopen") > 0) {
                    if ($(this).children().length > 2) {
                        $(this).find(">ul").hide('fast');
                        $(this).find("i:first").removeClass("i_treeopen").addClass("i_treeclose");
                    }

                    return false;
                }
            });
        }
    }
});