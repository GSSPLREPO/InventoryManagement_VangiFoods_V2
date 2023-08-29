﻿/*
 Highcharts JS v11.1.0 (2023-06-05)

 Mousewheel zoom module

 (c) 2023 Askel Eirik Johansson

 License: www.highcharts.com/license
*/
'use strict'; (function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/mouse-wheel-zoom", ["highcharts"], function (e) { a(e); a.Highcharts = e; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function e(a, g, e, n) { a.hasOwnProperty(g) || (a[g] = n.apply(null, e), "function" === typeof CustomEvent && window.dispatchEvent(new CustomEvent("HighchartsModuleLoaded", { detail: { path: g, module: a[g] } }))) }
    a = a ? a._modules : {}; e(a, "Extensions/MouseWheelZoom/MouseWheelZoom.js", [a["Core/Utilities.js"]], function (a) {
        function g() { const b = this, x = t(b.options.chart.zooming.mouseWheel); x.enabled && e(this.container, "wheel", a => { a = this.pointer.normalize(a); if (b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop)) { const f = u(x.sensitivity, 1.1); C(b, Math.pow(f, a.detail || (a.deltaY || 0) / 120), b.xAxis[0].toValue(a.chartX), b.yAxis[0].toValue(a.chartY), a.chartX, a.chartY, x) } a.preventDefault && a.preventDefault() }) } const { addEvent: e,
            isObject: n, pick: u, defined: f, merge: z } = a, A = [], B = { enabled: !0, sensitivity: 1.1 }, t = b => n(b) ? z(B, b) : z(B, { enabled: f(b) ? b : !0 }), D = function (b, a) { b.x + b.width > a.x + a.width && (b.width > a.width ? (b.width = a.width, b.x = a.x) : b.x = a.x + a.width - b.width); b.width > a.width && (b.width = a.width); b.x < a.x && (b.x = a.x); b.y + b.height > a.y + a.height && (b.height > a.height ? (b.height = a.height, b.y = a.y) : b.y = a.y + a.height - b.height); b.height > a.height && (b.height = a.height); b.y < a.y && (b.y = a.y); return b }; let y, r; const C = function (a, e, g, v, d, h, w) {
                const b = a.xAxis[0],
                c = a.yAxis[0]; var p = u(w.type, a.options.chart.zooming.type, "x"); w = /x/.test(p); p = /y/.test(p); if (f(b.max) && f(b.min) && f(c.max) && f(c.min) && f(b.dataMax) && f(b.dataMin) && f(c.dataMax) && f(c.dataMin)) {
                    if (p) { f(y) && clearTimeout(y); const { startOnTick: a, endOnTick: b } = c.options; r || (r = { startOnTick: a, endOnTick: b }); (a || b) && c.setOptions({ startOnTick: !1, endOnTick: !1 }); y = setTimeout(() => { if (r) { c.setOptions(r); const { min: a, max: b } = c.getExtremes(); c.forceRedraw = !0; c.setExtremes(a, b); r = void 0 } }, 400) } if (a.inverted) {
                        var q = c.pos +
                            c.len; g = b.toValue(h); v = c.toValue(d); var k = d; d = h; h = q - k + c.pos
                    } d = d ? (d - b.pos) / b.len : .5; if (b.reversed && !a.inverted || a.inverted && !b.reversed) d = 1 - d; h = 1 - (h ? (h - c.pos) / c.len : .5); c.reversed && (h = 1 - h); q = b.max - b.min; g = u(g, b.min + q / 2); q *= e; k = c.max - c.min; v = u(v, c.min + k / 2); const t = k * e; var l = b.dataMax - b.dataMin, m = c.dataMax - c.dataMin; k = b.dataMin - l * b.options.minPadding; l = l + l * b.options.minPadding + l * b.options.maxPadding; const n = c.dataMin - m * c.options.minPadding; m = m + m * c.options.minPadding + m * c.options.maxPadding; d = D({
                        x: g - q *
                            d, y: v - t * h, width: q, height: t
                    }, { x: k, y: n, width: l, height: m }); h = d.x <= k && d.width >= l && d.y <= n && d.height >= m; f(e) && !h ? (w && b.setExtremes(d.x, d.x + d.width, !1), p && c.setExtremes(d.y, d.y + d.height, !1)) : (w && b.setExtremes(void 0, void 0, !1), p && c.setExtremes(void 0, void 0, !1)); a.redraw(!1)
                }
            }; ""; return { compose: function (a) { -1 === A.indexOf(a) && (A.push(a), e(a, "afterGetContainer", g)) } }
    }); e(a, "masters/modules/mouse-wheel-zoom.src.js", [a["Core/Globals.js"], a["Extensions/MouseWheelZoom/MouseWheelZoom.js"]], function (a, e) { e.compose(a.Chart) })
});
//# sourceMappingURL=mouse-wheel-zoom.js.map