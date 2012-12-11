$ ->
	$.fn.disable = ->
		@each ->
			$this = $(this)
			$this.attr('disabled', 'disabled')

	$.fn.enable = ->
		@each ->
			$this = $(this)
			$this.removeAttr('disabled')