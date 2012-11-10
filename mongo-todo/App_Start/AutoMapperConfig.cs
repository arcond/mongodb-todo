using AutoMapper;
using Domain.Model;
using mongo_todo.Models;

namespace mongo_todo
{
	public static class AutoMapperConfig
	{
		public static void RegisterMaps()
		{
			Mapper.CreateMap<Task, TaskModel>()
				.ForMember(x => x.Id, x => x.MapFrom(y => y.Id.ToString()));

			Mapper.CreateMap<User, UserModel>()
				.ForMember(x => x.Id, x => x.MapFrom(y => y.Id.ToString()))
				.ForMember(x => x.TasksUrl, x => x.Ignore())
				.AfterMap((y, x) => {
					x.TasksUrl = string.Concat("~/tasks/all/", x.Id);
				});
		}
	}
}