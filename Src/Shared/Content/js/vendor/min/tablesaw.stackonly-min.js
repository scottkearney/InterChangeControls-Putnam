(function($) {
    var div = document.createElement("div"), all = div.getElementsByTagName("i"), $doc = $(document.documentElement);
    div.innerHTML = "<!--[if lte IE 8]><i></i><![endif]-->";
    if (all[0]) {
        $doc.addClass("ie-lte8");
    }
    if (!("querySelector" in document) || window.blackberry && !window.WebKitPoint || window.operamini) {
        return;
    } else {
        $doc.addClass("tablesaw-enhanced");
        $(function() {
            $(document).trigger("enhance.tablesaw");
        });
    }
})(jQuery);

(function($) {
    var pluginName = "table", classes = {
        toolbar:"tablesaw-bar"
    }, events = {
        create:"tablesawcreate",
        destroy:"tablesawdestroy",
        refresh:"tablesawrefresh"
    }, defaultMode = "stack", initSelector = "table[data-mode],table[data-sortable]";
    var Table = function(element) {
        if (!element) {
            throw new Error("Tablesaw requires an element.");
        }
        this.table = element;
        this.$table = $(element);
        this.mode = this.$table.attr("data-mode") || defaultMode;
        this.init();
    };
    Table.prototype.init = function() {
        if (!this.$table.attr("id")) {
            this.$table.attr("id", pluginName + "-" + Math.round(Math.random() * 1e4));
        }
        this.createToolbar();
        var colstart = this._initCells();
        this.$table.trigger(events.create, [ this, colstart ]);
    };
    Table.prototype._initCells = function() {
        var colstart, thrs = this.table.querySelectorAll("thead tr"), self = this;
        $(thrs).each(function() {
            var coltally = 0;
            $(this).children().each(function() {
                var span = parseInt(this.getAttribute("colspan"), 10), sel = ":nth-child(" + (coltally + 1) + ")";
                colstart = coltally + 1;
                if (span) {
                    for (var k = 0; k < span - 1; k++) {
                        coltally++;
                        sel += ", :nth-child(" + (coltally + 1) + ")";
                    }
                }
                this.cells = self.$table.find("tr").not($(thrs).eq(0)).not(this).children(sel);
                coltally++;
            });
        });
        return colstart;
    };
    Table.prototype.refresh = function() {
        this._initCells();
        this.$table.trigger(events.refresh);
    };
    Table.prototype.createToolbar = function() {
        var $toolbar = this.$table.prev("." + classes.toolbar);
        if (!$toolbar.length) {
            $toolbar = $("<div>").addClass(classes.toolbar).insertBefore(this.$table);
        }
        this.$toolbar = $toolbar;
        if (this.mode) {
            this.$toolbar.addClass("mode-" + this.mode);
        }
    };
    Table.prototype.destroy = function() {
        this.$table.prev("." + classes.toolbar).each(function() {
            this.className = this.className.replace(/\bmode\-\w*\b/gi, "");
        });
        var tableId = this.$table.attr("id");
        $(document).unbind("." + tableId);
        $(window).unbind("." + tableId);
        this.$table.trigger(events.destroy, [ this ]);
        this.$table.removeAttr("data-mode");
        this.$table.removeData(pluginName);
    };
    $.fn[pluginName] = function() {
        return this.each(function() {
            var $t = $(this);
            if ($t.data(pluginName)) {
                return;
            }
            var table = new Table(this);
            $t.data(pluginName, table);
        });
    };
    $(document).on("enhance.tablesaw", function(e) {
        $(e.target).find(initSelector)[pluginName]();
    });
})(jQuery);

(function(win, $, undefined) {
    var classes = {
        stackTable:"tablesaw-stack",
        cellLabels:"tablesaw-cell-label"
    };
    var data = {
        obj:"tablesaw-stack"
    };
    var attrs = {
        labelless:"data-no-labels"
    };
    var Stack = function(element) {
        this.$table = $(element);
        this.labelless = this.$table.is("[" + attrs.labelless + "]");
        if (!this.labelless) {
            this.allHeaders = this.$table.find("th");
        }
        this.$table.data(data.obj, this);
    };
    Stack.prototype.init = function(colstart) {
        this.$table.addClass(classes.stackTable);
        if (this.labelless) {
            return;
        }
        var reverseHeaders = $(this.allHeaders);
        reverseHeaders.each(function() {
            var $cells = $(this.cells).filter(function() {
                return !$(this).parent().is("[" + attrs.labelless + "]");
            }), hierarchyClass = $cells.not(this).filter("thead th").length && " tablesaw-cell-label-top", text = $(this).text();
            if (text !== "") {
                if (hierarchyClass) {
                    var iteration = parseInt($(this).attr("colspan"), 10), filter = "";
                    if (iteration) {
                        filter = "td:nth-child(" + iteration + "n + " + colstart + ")";
                    }
                    $cells.filter(filter).prepend("<b class='" + classes.cellLabels + hierarchyClass + "'>" + text + "</b>");
                } else {
                    $cells.prepend("<b class='" + classes.cellLabels + "'>" + text + "</b>");
                }
            }
        });
    };
    Stack.prototype.destroy = function() {
        this.$table.removeClass(classes.stackTable);
        this.$table.find("." + classes.cellLabels).remove();
    };
    $(document).on("tablesawcreate", function(e, Tablesaw, colstart) {
        if (Tablesaw.mode === "stack") {
            var table = new Stack(Tablesaw.table);
            table.init(colstart);
        }
    });
    $(document).on("tablesawdestroy", function(e, Tablesaw) {
        if (Tablesaw.mode === "stack") {
            $(Tablesaw.table).data(data.obj).destroy();
        }
    });
})(this, jQuery);