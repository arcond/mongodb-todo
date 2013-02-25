define [
	'underscore'
	'backbone'
], (_, Backbone) ->
	class BaseModel extends Backbone.Model
		constructor: (options) ->
			super options
			return

	class User extends BaseModel
		urlRoot: '/api/users'
		defaults:
			name: ''

		updateName: (newName) ->
			@set
				name: newName

	class Todo extends BaseModel
		defaults:
			description: ''
			completed: false

		initialize: (options) ->
			if options?.urlRoot
				@urlRoot = ->
					options.urlRoot
			super options

		updateDescription: (newDescription) ->
			@set 'description', newDescription
			return

		toggle: ->
			@save 'completed', not @get 'completed'
			return

	{
		User: User
		Todo: Todo
	}