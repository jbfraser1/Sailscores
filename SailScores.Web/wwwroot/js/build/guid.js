"use strict";function _classCallCheck(e,r){if(!(e instanceof r))throw new TypeError("Cannot call a class as a function")}function _defineProperties(e,r){for(var t=0;t<r.length;t++){var n=r[t];n.enumerable=n.enumerable||!1,n.configurable=!0,"value"in n&&(n.writable=!0),Object.defineProperty(e,n.key,n)}}function _createClass(e,r,t){return r&&_defineProperties(e.prototype,r),t&&_defineProperties(e,t),e}Object.defineProperty(exports,"__esModule",{value:!0}),exports.Guid=void 0;var Guid=function(){function e(r){_classCallCheck(this,e),this.guid=r,this._guid=r}return _createClass(e,[{key:"ToString",value:function(){return this.guid}}],[{key:"MakeNew",value:function(){var r,t;for(r="",t=0;t<32;t++)8!=t&&12!=t&&16!=t&&20!=t||(r+="-"),r+=Math.floor(16*Math.random()).toString(16).toUpperCase();return new e(r)}}]),e}();exports.Guid=Guid;