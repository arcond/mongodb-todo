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

			# flag or un-flag an invalid model
			if @isValid() is false and not @has('invalid')
				@set 'invalid', @isValid()
			else if @isValid() and @has('invalid')
				@unset 'invalid',
					silent: true

			# flag or un-flag a dirty model
			if @isDirty() and not @has('dirty')
				@set 'dirty', @isDirty()
			else if not @isDirty() and @has('dirty')
				@unset 'dirty',
					silent: true
			return

	class User extends BaseModel
		urlRoot: '/api/users'
		defaults:
			name: ''
			tasksUrl: ''

		updateName: (newName) ->
			@set
				name: newName

	class Todo extends BaseModel
		urlRoot: ->
			'/api/users/#{@get("userId")}/tasks'
		defaults:
			description: ''
			completed: false

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