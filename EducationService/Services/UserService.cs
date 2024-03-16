using EducationService.Models;
using EducationService.Repositories;

namespace EducationService.Services;

public class UserService
{
    private readonly UserRepository userRepository;

    public UserService(UserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<User?> AddUser(User user)
    {
        return await userRepository.AddUser(user);
    }
    

    public async Task<User?> GetById(int id)
    {
        return await userRepository.GetById(id);
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await userRepository.GetByUsername(username);
    }
}
