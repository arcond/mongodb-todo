define [
	'underscore'
	'backbone'
], (_, Backbone) ->
	class User extends Backbone.Model
		urlRoot: '/api/users'
		defaults:
			name: ''

		updateName: (newName) ->
			@set
				name: newName

		toJSON: (options) ->
			json = super()
			if options?.edit
				json.editing = true
			json

	class Todo extends Backbone.Model
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