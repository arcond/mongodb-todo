// Generated by CoffeeScript 1.6.2
(function() {
  $(function() {
    $.fn.disable = function() {
      return this.each(function() {
        var $this;

        $this = $(this);
        return $this.attr('disabled', 'disabled');
      });
    };
    return $.fn.enable = function() {
      return this.each(function() {
        var $this;

        $this = $(this);
        return $this.removeAttr('disabled');
      });
    };
  });

}).call(this);
