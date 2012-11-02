define [
	'jquery'
	'underscore'
	'backbone'
	'views'
	'collections'
	'models'
], ($, _, Backbone, Views, Collections, Models) ->
	class Router extends Backbone.Router
		$el: null
		routes:
			'': 'root'
			':userId': 'tasks'

		initialize: (options) ->
			@$el = $('#content')
			@users = new Collections.Users
			@toolbarView = new Views.ToolbarView collection: @users
			@userView = new Views.UserView
			super options

		setup: ->
			@toolbarView.remove()
			@toolbarView.on 'users:add', =>
				Backbone.history.navigate "#0", true
				return
			, @
			@toolbarView.on 'users:select', (userModel) =>
				Backbone.history.navigate "##{userModel.id}", true
				return
			, @
			@toolbarView.on 'save-all', =>
				@users.save()
				return
			, @
			@$el.html @toolbarView.render().el

			@users.fetch()
			return			

		root: ->
			@setup()
			return

		tasks: (userId) ->
			@userView.remove()
			@users.on 'reset', =>
				if userId > 0 then user = @users.get userId
				else user = new Models.User
				@userView = new Views.UserView model: user
				@$el.find('#user').html @userView.render().el
				return
			, @
			@setup()
			return

	Router