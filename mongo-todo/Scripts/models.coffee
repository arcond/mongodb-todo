define [
	'underscore'
	'backbone'
], (_, Backbone) ->
	class BaseModel extends Backbone.Model

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
			id: ''
			description: ''
			completed: false
			userId: ''

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