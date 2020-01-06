(function() {
  var iced,
    __slice = [].slice;

  iced = {
    Deferrals: (function() {
      function _Class(_arg) {
        this.continuation = _arg;
        this.count = 1;
        this.ret = null;
      }

      _Class.prototype._fulfill = function() {
        if (!--this.count) {
          return this.continuation(this.ret);
        }
      };

      _Class.prototype.defer = function(defer_params) {
        ++this.count;
        return (function(_this) {
          return function() {
            var inner_params, _ref;
            inner_params = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
            if (defer_params != null) {
              if ((_ref = defer_params.assign_fn) != null) {
                _ref.apply(null, inner_params);
              }
            }
            return _this._fulfill();
          };
        })(this);
      };

      return _Class;

    })(),
    findDeferral: function() {
      return null;
    },
    trampoline: function(_fn) {
      return _fn();
    }
  };

  this.isAvailable = function(type, value) {
    var deferred;
    deferred = $q.defer({
      lineno: 1,
      context: __iced_deferrals
    });
    $http.get(prApiRoot + "users/available?" + type + "=" + value).success(function(data) {
      deferred.resolve(data);
    }).error(function(data, status) {
      deferred.reject({
        data: data,
        status: status
      });
    });
    return deferred.promise;
  };

}).call(this);
