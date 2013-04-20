define [
	'underscore'
	'backbone'
	'models'
], (_, Backbone, Models) ->
	class BaseCollection extends Backbone.Collection
		save: (options) ->
			@set @models, options
			method = if options?.force then 'update' else 'patch'
			xhr = @sync method, @, options
			xhr

	class Users extends BaseCollection
		model: Models.User
		url: '/api/users'

	class Todos extends BaseCollection
		model: Models.Todo
		url: ''

		initialize: (options) ->
			@url = options.url if options?.url
			super options

		completed: ->
			@filter (model) ->
				model.get('completed') is true

		remaining: ->
			@filter (model) ->
				model.get('completed') is false

	{
		Users: Users
		Todos: Todos
	}