import axios from 'axios';
const apiUrl =process.env.REACT_APP_API;
const apiClient=axios.create({
  baseURL:process.env.REACT_APP_API,
});
axios.defaults.baseURL=apiUrl;


export default {
  getTasks: async () => {
    console.log('get task')
    const result = await axios.get()
    console.log(result)
    return   result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name)
    const result = await axios.post(``, { Name: name, IsComplete: false })
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete })
    const result = await axios.put(`${id}`, { isComplete: isComplete })
    return result.data;
  },

  deleteTask: async (id) => {
    console.log('deleteTask')
    const result = await axios.delete(`/${id}`)
    return result.data;
  }
};