define [
	'underscore'
	'backbone'
], (_, Backbone) ->
	class BaseModel extends Backbone.Model
		_preservedAttributes: {}

		constructor: (attributes, options) ->
			super attributes, options
			@_preservedAttributes = @toJSON()
			return

		parse: (response) ->
			response = super response
			@_preservedAttributes = @toJSON()
			response

		isDirty: ->
			not _.isEqual @toJSON(), @_preservedAttributes

		set: (attributes, options) ->
			super attributes, options
			if @isValid() is false and not @has 'invalid'
				@set 'invalid', @isValid()
			else if @isValid() is true and @has 'invalid'
				@unset 'invalid',
					silent: true
			return

	class User extends BaseModel
		urlRoot: '/api/users/user'
		defaults:
			name: ''
			tasksUrl: ''

		updateName: (newName) ->
			@set
				name: newName

	class Todo extends BaseModel
		urlRoot: '/api/tasks/task'
		defaults:
			description: ''
			completed: false

		updateDescription: (newDescription) ->
			@set 'description', newDescription
			return

		toggle: ->
			@save 'completed', not @get 'completed'
			return

	# Models = Models ? {}
	# Models.User = User
	# Models.Todo = Todo
	# Models
	{
		User: User
		Todo: Todo
	}